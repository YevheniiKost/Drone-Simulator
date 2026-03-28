using UnityEngine;

using DroneSimulator.Config;
using DroneSimulator.Flight;
using DroneSimulator.Input;
using DroneSimulator.Sensors;

namespace DroneSimulator
{
    // Entry point for the quadcopter simulation.
    // Responsibilities:
    //   - load configs from Resources
    //   - wire up the input provider, sensor reader, and flight controller
    //   - apply the computed motor forces and external forces to the Rigidbody
    //
    // All flight logic lives in QuadcopterFlightController (plain C# class).
    // All input reading lives in a component that implements IDroneInputProvider.
    // All sensor reading lives in DroneSensors (plain C# class).
    [RequireComponent(typeof(Rigidbody))]
    public class QuadcopterView : MonoBehaviour
    {
        private const string PhysicsConfigResourcePath = "Config/DronePhysicsConfig";
        private const string PidConfigResourcePath = "Config/DronePIDConfig";

        [SerializeField] private GameObject _propellerFrontRight;
        [SerializeField] private GameObject _propellerFrontLeft;
        [SerializeField] private GameObject _propellerBackRight;
        [SerializeField] private GameObject _propellerBackLeft;

        private Rigidbody _rigidbody;
        private IDroneInputProvider _inputProvider;
        private DroneSensors _sensors;
        private QuadcopterFlightController _flightController;
        private DronePhysicsConfig _physicsConfig;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _inputProvider = GetComponent<IDroneInputProvider>();

            _physicsConfig = Resources.Load<DronePhysicsConfig>(PhysicsConfigResourcePath);
            DronePidConfig pidConfig = Resources.Load<DronePidConfig>(PidConfigResourcePath);

            if (_physicsConfig == null)
            {
                Debug.LogError(
                    $"[QuadcopterView] DronePhysicsConfig not found at Resources/{PhysicsConfigResourcePath}. " +
                    "Create it via Assets > Create > DroneSimulator > Drone Physics Config " +
                    "and place it under Assets/Resources/DroneSimulator/.");
            }

            if (pidConfig == null)
            {
                Debug.LogError(
                    $"[QuadcopterView] DronePIDConfig not found at Resources/{PidConfigResourcePath}. " +
                    "Create it via Assets > Create > DroneSimulator > Drone PID Config " +
                    "and place it under Assets/Resources/DroneSimulator/.");
            }

            if (_inputProvider == null)
            {
                Debug.LogError(
                    "[QuadcopterView] No IDroneInputProvider component found on this GameObject. " +
                    "Add a KeyboardDroneInputProvider (or any other provider) to the same object.");
            }

            if (_physicsConfig != null && pidConfig != null)
            {
                _sensors = new DroneSensors(transform, _rigidbody);
                _flightController = new QuadcopterFlightController(_physicsConfig, pidConfig);
            }
        }

        private void FixedUpdate()
        {
            if (_flightController == null || _inputProvider == null)
            {
                return;
            }

            DroneInputState inputState = _inputProvider.ReadInput();
            DroneSensorData sensorData = _sensors.ReadSensorData();
            DroneMotorOutput motorOutput = _flightController.ComputeMotorOutput(sensorData, inputState);

            ApplyMotorForces(motorOutput);
            ApplyExternalForces();
        }

        private void ApplyMotorForces(DroneMotorOutput motorOutput)
        {
            ApplyPropellerForce(_propellerFrontRight, motorOutput.FrontRightForce);
            ApplyPropellerForce(_propellerFrontLeft, motorOutput.FrontLeftForce);
            ApplyPropellerForce(_propellerBackRight, motorOutput.BackRightForce);
            ApplyPropellerForce(_propellerBackLeft, motorOutput.BackLeftForce);

            _rigidbody.AddTorque(transform.up * motorOutput.YawTorque);
        }

        private void ApplyPropellerForce(GameObject propellerObject, float force)
        {
            _rigidbody.AddForceAtPosition(
                propellerObject.transform.up * force,
                propellerObject.transform.position);
        }

        private void ApplyExternalForces()
        {
            Vector3 windDirection = Quaternion.Euler(0f, _physicsConfig.WindDirection, 0f) * (-Vector3.forward);
            _rigidbody.AddForce(windDirection * _physicsConfig.WindForce);

            Debug.DrawRay(transform.position, -windDirection * 3f, Color.red);
        }
    }
}

