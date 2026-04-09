using DroneSimulator.Data.Input;
using DroneSimulator.Domain.Flight;

namespace DroneSimulator.Domain.Simulation
{
    public readonly struct DroneRealtimeData
    {
        public readonly float Height;
        public readonly float Speed;
        public readonly DroneInputState Input;
        public readonly FlightPidDebugTelemetry PidDebug;

        public DroneRealtimeData(
            float height,
            float speed,
            DroneInputState input,
            FlightPidDebugTelemetry pidDebug = default)
        {
            Height = height;
            Speed = speed;
            Input = input;
            PidDebug = pidDebug;
        }
    }
}
