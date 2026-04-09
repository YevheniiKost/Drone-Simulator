using UnityEngine;

using DroneSimulator.Data.Config;
using DroneSimulator.Data.Input;
using DroneSimulator.Data.Sensors;

namespace DroneSimulator.Domain.Flight
{
    public class AcroFlightController : IFlightController
    {
        private readonly DronePhysicsConfig _physicsConfig;
        private readonly DroneAcroConfig _acroConfig;

        private readonly PIDController _pitchRatePid;
        private readonly PIDController _rollRatePid;
        private readonly PIDController _yawRatePid;

        public AcroFlightController(DronePhysicsConfig physicsConfig, DroneAcroConfig acroConfig)
        {
            _physicsConfig = physicsConfig;
            _acroConfig = acroConfig;

            _pitchRatePid = new PIDController(
                acroConfig.PitchRateGains.x,
                acroConfig.PitchRateGains.y,
                acroConfig.PitchRateGains.z,
                acroConfig.RateErrorSumMax);

            _rollRatePid = new PIDController(
                acroConfig.RollRateGains.x,
                acroConfig.RollRateGains.y,
                acroConfig.RollRateGains.z,
                acroConfig.RateErrorSumMax);

            _yawRatePid = new PIDController(
                acroConfig.YawRateGains.x,
                acroConfig.YawRateGains.y,
                acroConfig.YawRateGains.z,
                acroConfig.RateErrorSumMax);
        }

        public DroneMotorOutput ComputeMotorOutput(DroneSensorData sensorData, DroneInputState inputState)
        {
            float baseThrottle = inputState.Throttle * _physicsConfig.MaxPropellerForce;

            float pitchCorrection = ComputePitchRateCorrection(inputState.Pitch, sensorData.PitchAngularVelocity);
            float rollCorrection = ComputeRollRateCorrection(inputState.Roll, sensorData.RollAngularVelocity);
            float yawCorrection = ComputeYawRateCorrection(inputState.Yaw, sensorData.YawAngularVelocity);

            DroneMotorOutput output = new DroneMotorOutput();

            output.FrontRightForce = ClampPropellerForce(baseThrottle - pitchCorrection - rollCorrection);
            output.FrontLeftForce = ClampPropellerForce(baseThrottle - pitchCorrection + rollCorrection);
            output.BackRightForce = ClampPropellerForce(baseThrottle + pitchCorrection - rollCorrection);
            output.BackLeftForce = ClampPropellerForce(baseThrottle + pitchCorrection + rollCorrection);

            output.YawTorque = yawCorrection * _physicsConfig.MaxTorque;

            return output;
        }

        private float ComputePitchRateCorrection(float pitchStick, float pitchAngularVelocity)
        {
            float desiredPitchRate = pitchStick * _acroConfig.MaxPitchRate;
            float pitchRateError = desiredPitchRate - pitchAngularVelocity;
            return _pitchRatePid.Compute(pitchRateError);
        }

        private float ComputeRollRateCorrection(float rollStick, float rollAngularVelocity)
        {
            float desiredRollRate = rollStick * _acroConfig.MaxRollRate;
            float rollRateError = desiredRollRate - rollAngularVelocity;
            return _rollRatePid.Compute(rollRateError);
        }

        private float ComputeYawRateCorrection(float yawStick, float yawAngularVelocityRadPerSec)
        {
            float desiredYawRate = yawStick * _acroConfig.MaxYawRate;
            float actualYawRateDegPerSec = yawAngularVelocityRadPerSec * Mathf.Rad2Deg;
            float yawRateError = desiredYawRate - actualYawRateDegPerSec;
            return _yawRatePid.Compute(yawRateError);
        }

        private float ClampPropellerForce(float force)
        {
            return Mathf.Clamp(force, 0f, _physicsConfig.MaxPropellerForce);
        }
    }
}
