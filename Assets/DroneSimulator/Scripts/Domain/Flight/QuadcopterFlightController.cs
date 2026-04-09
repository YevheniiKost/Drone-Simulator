using UnityEngine;

using DroneSimulator.Data.Config;
using DroneSimulator.Data.Input;
using DroneSimulator.Data.Sensors;

namespace DroneSimulator.Domain.Flight
{
    public class QuadcopterFlightController : IFlightController
    {
        private readonly DronePhysicsConfig _physicsConfig;
        private readonly DronePidConfig _pidConfig;

        private readonly PIDController _pitchPid;
        private readonly PIDController _rollPid;
        private readonly PIDController _yawPid;

        private float _currentThrottle;

        public float CurrentThrottle => _currentThrottle;

        public QuadcopterFlightController(DronePhysicsConfig physicsConfig, DronePidConfig pidConfig)
        {
            _physicsConfig = physicsConfig;
            _pidConfig = pidConfig;

            _pitchPid = new PIDController(
                pidConfig.PitchGains.x,
                pidConfig.PitchGains.y,
                pidConfig.PitchGains.z,
                pidConfig.ErrorSumMax);

            _rollPid = new PIDController(
                pidConfig.RollGains.x,
                pidConfig.RollGains.y,
                pidConfig.RollGains.z,
                pidConfig.ErrorSumMax);

            _yawPid = new PIDController(
                pidConfig.YawGains.x,
                pidConfig.YawGains.y,
                pidConfig.YawGains.z,
                pidConfig.ErrorSumMax);
        }

        public DroneMotorOutput ComputeMotorOutput(DroneSensorData sensorData, DroneInputState inputState)
        {
            UpdateThrottle(inputState.Throttle);

            float pitchCorrection = ComputePitchCorrection(sensorData.PitchAngle);
            float rollCorrection = _rollPid.Compute(sensorData.RollAngle);

            DroneMotorOutput output = new DroneMotorOutput();

            output.FrontRightForce = ComputePropellerForce(
                _currentThrottle, pitchCorrection, rollCorrection,
                -inputState.Pitch, -inputState.Roll);

            output.FrontLeftForce = ComputePropellerForce(
                _currentThrottle, pitchCorrection, -rollCorrection,
                -inputState.Pitch, inputState.Roll);

            output.BackRightForce = ComputePropellerForce(
                _currentThrottle, -pitchCorrection, rollCorrection,
                inputState.Pitch, -inputState.Roll);

            output.BackLeftForce = ComputePropellerForce(
                _currentThrottle, -pitchCorrection, -rollCorrection,
                inputState.Pitch, inputState.Roll);

            float yawSteer = inputState.Yaw * _physicsConfig.MaxTorque * _currentThrottle;
            float yawDamping = _yawPid.Compute(sensorData.YawAngularVelocity) * _currentThrottle;
            output.YawTorque = yawSteer - yawDamping;

            return output;
        }

        private float ComputePitchCorrection(float pitchAngle)
        {
            float halfThrottle = _physicsConfig.ThrottleMax * 0.5f;

            if (_currentThrottle > halfThrottle)
            {
                _pitchPid.UpdateGains(
                    _pidConfig.PitchGains.x * 2f,
                    _pidConfig.PitchGains.y * 2f,
                    _pidConfig.PitchGains.z * 2f);
            }
            else
            {
                _pitchPid.UpdateGains(
                    _pidConfig.PitchGains.x,
                    _pidConfig.PitchGains.y,
                    _pidConfig.PitchGains.z);
            }

            return _pitchPid.Compute(pitchAngle);
        }

        private float ComputePropellerForce(
            float baseThrottle,
            float pitchContribution,
            float rollContribution,
            float forwardBackSteering,
            float leftRightSteering)
        {
            float force = baseThrottle
                + pitchContribution
                + rollContribution
                + forwardBackSteering * baseThrottle * _physicsConfig.MoveFactor
                + leftRightSteering * baseThrottle;

            return Mathf.Clamp(force, 0f, _physicsConfig.MaxPropellerForce);
        }

        private void UpdateThrottle(float throttleDelta)
        {
            if (throttleDelta != 0f)
            {
                _currentThrottle += throttleDelta * _physicsConfig.ThrottleStep;
            }
            else
            {
                _currentThrottle = Mathf.MoveTowards(
                    _currentThrottle,
                    _physicsConfig.IdleThrottle,
                    _physicsConfig.ThrottleDecayStep);
            }

            _currentThrottle = Mathf.Clamp(_currentThrottle, 0f, _physicsConfig.ThrottleMax);
        }
    }
}
