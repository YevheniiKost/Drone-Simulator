using DroneSimulator.Domain;

namespace DroneSimulator.Domain.Flight
{
    public enum FlightPidDebugMode
    {
        None = 0,
        AcroRate = 1,
        StabilisedAngle = 2
    }

    /// <summary>
    /// Snapshot for HUD / tuning: sensor context, loop errors, PID P/I/D, mixer or torque outputs.
    /// </summary>
    public readonly struct FlightPidDebugTelemetry
    {
        public readonly FlightPidDebugMode Mode;

        public readonly float PitchAngleDeg;
        public readonly float RollAngleDeg;
        public readonly float PitchRateDegS;
        public readonly float RollRateDegS;
        public readonly float YawRateDegS;

        public readonly float PitchError;
        public readonly float RollError;
        public readonly float YawError;

        public readonly PidLastCompute PitchPid;
        public readonly PidLastCompute RollPid;
        public readonly PidLastCompute YawPid;

        public readonly float PitchMixerOut;
        public readonly float RollMixerOut;
        public readonly float YawTorqueOut;
        public readonly float YawFeedForwardTorque;

        public FlightPidDebugTelemetry(
            FlightPidDebugMode mode,
            float pitchAngleDeg,
            float rollAngleDeg,
            float pitchRateDegS,
            float rollRateDegS,
            float yawRateDegS,
            float pitchError,
            float rollError,
            float yawError,
            PidLastCompute pitchPid,
            PidLastCompute rollPid,
            PidLastCompute yawPid,
            float pitchMixerOut,
            float rollMixerOut,
            float yawTorqueOut,
            float yawFeedForwardTorque)
        {
            Mode = mode;
            PitchAngleDeg = pitchAngleDeg;
            RollAngleDeg = rollAngleDeg;
            PitchRateDegS = pitchRateDegS;
            RollRateDegS = rollRateDegS;
            YawRateDegS = yawRateDegS;
            PitchError = pitchError;
            RollError = rollError;
            YawError = yawError;
            PitchPid = pitchPid;
            RollPid = rollPid;
            YawPid = yawPid;
            PitchMixerOut = pitchMixerOut;
            RollMixerOut = rollMixerOut;
            YawTorqueOut = yawTorqueOut;
            YawFeedForwardTorque = yawFeedForwardTorque;
        }
    }
}
