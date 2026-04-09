using DroneSimulator.Data.Config;
using DroneSimulator.Data.Input;
using DroneSimulator.Data.Sensors;
using DroneSimulator.Domain.Flight;
using DroneSimulator.Domain.Simulation;

using UnityEngine;

namespace DroneSimulator.Presentation.Simulation
{
    [RequireComponent(typeof(Rigidbody))]
    public class QuadcopterView : MonoBehaviour
    {
        private const string PhysicsConfigResourcePath = "Config/DronePhysicsConfig";
        private const string PidConfigResourcePath = "Config/DronePIDConfig";
        private const string AcroConfigResourcePath = "Config/DroneAcroConfig";

        [SerializeField]
        private GameObject _propellerFrontRight;
        [SerializeField]
        private GameObject _propellerFrontLeft;
        [SerializeField]
        private GameObject _propellerBackRight;
        [SerializeField]
        private GameObject _propellerBackLeft;

        [SerializeField]
        private FlightMode _flightMode;

        private Rigidbody _rigidbody;
        private IDroneInputProvider _inputProvider;
        private DroneSensors _sensors;
        private IFlightController _flightController;
        private DronePhysicsConfig _physicsConfig;
        private IConfigProvider _configProvider;
        private ISimulationModel _simulationModel;

        public void Configure(IConfigProvider configProvider, ISimulationModel simulationModel)
        {
            _configProvider = configProvider;
            _simulationModel = simulationModel;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _inputProvider = GetComponent<IDroneInputProvider>();

            DronePhysicsConfig physicsConfig = ResolvePhysicsConfig();
            DronePidConfig pidConfig = ResolvePidConfig();
            DroneAcroConfig acroConfig = ResolveAcroConfig();

            _physicsConfig = physicsConfig;

            LogConfigErrors(physicsConfig, pidConfig, acroConfig);

            if (!CanInitialiseController(physicsConfig, pidConfig, acroConfig))
            {
                return;
            }

            _sensors = new DroneSensors(transform, _rigidbody);
            _flightController = CreateFlightController(physicsConfig, pidConfig, acroConfig);
        }

        private DronePhysicsConfig ResolvePhysicsConfig()
        {
            if (_configProvider != null)
            {
                return _configProvider.GetPhysicsConfig();
            }

            return Resources.Load<DronePhysicsConfig>(PhysicsConfigResourcePath);
        }

        private DronePidConfig ResolvePidConfig()
        {
            if (_configProvider != null)
            {
                return _configProvider.GetPidConfig();
            }

            return Resources.Load<DronePidConfig>(PidConfigResourcePath);
        }

        private DroneAcroConfig ResolveAcroConfig()
        {
            if (_configProvider != null)
            {
                return _configProvider.GetAcroConfig();
            }

            return Resources.Load<DroneAcroConfig>(AcroConfigResourcePath);
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

            PushRealtimeData(sensorData, inputState);
        }

        private void PushRealtimeData(DroneSensorData sensorData, DroneInputState inputState)
        {
            if (_simulationModel == null)
            {
                return;
            }

            DroneRealtimeData data = new DroneRealtimeData(sensorData.Height, sensorData.Speed, inputState);
            _simulationModel.UpdateDroneRealtimeData(data);
        }

        private void ApplyMotorForces(DroneMotorOutput motorOutput)
        {
            ApplyPropellerForce(_propellerFrontRight, motorOutput.FrontRightForce);
            ApplyPropellerForce(_propellerFrontLeft, motorOutput.FrontLeftForce);
            ApplyPropellerForce(_propellerBackRight, motorOutput.BackRightForce);
            ApplyPropellerForce(_propellerBackLeft, motorOutput.BackLeftForce);

            _rigidbody.AddTorque(Vector3.up * motorOutput.YawTorque);
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

        private void LogConfigErrors(
            DronePhysicsConfig physicsConfig,
            DronePidConfig pidConfig,
            DroneAcroConfig acroConfig)
        {
            if (physicsConfig == null)
            {
                Debug.LogError(
                    "[QuadcopterView] DronePhysicsConfig missing. Expected Resources/" + PhysicsConfigResourcePath + ".");
            }

            if (_inputProvider == null)
            {
                Debug.LogError("[QuadcopterView] No IDroneInputProvider on this GameObject.");
            }

            if (_flightMode == FlightMode.Stabilised && pidConfig == null)
            {
                Debug.LogError(
                    "[QuadcopterView] DronePIDConfig missing. Expected Resources/" + PidConfigResourcePath + ".");
            }

            if (_flightMode == FlightMode.Acro && acroConfig == null)
            {
                Debug.LogError(
                    "[QuadcopterView] DroneAcroConfig missing. Expected Resources/" + AcroConfigResourcePath + ".");
            }
        }

        private bool CanInitialiseController(
            DronePhysicsConfig physicsConfig,
            DronePidConfig pidConfig,
            DroneAcroConfig acroConfig)
        {
            if (physicsConfig == null)
            {
                return false;
            }

            if (_flightMode == FlightMode.Stabilised)
            {
                return pidConfig != null;
            }

            if (_flightMode == FlightMode.Acro)
            {
                return acroConfig != null;
            }

            return false;
        }

        private IFlightController CreateFlightController(
            DronePhysicsConfig physicsConfig,
            DronePidConfig pidConfig,
            DroneAcroConfig acroConfig)
        {
            if (_flightMode == FlightMode.Stabilised)
            {
                return new QuadcopterFlightController(physicsConfig, pidConfig);
            }

            return new AcroFlightController(physicsConfig, acroConfig);
        }
    }
}
