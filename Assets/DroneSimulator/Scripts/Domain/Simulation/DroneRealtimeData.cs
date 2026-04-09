using DroneSimulator.Data.Input;

namespace DroneSimulator.Domain.Simulation
{
    public readonly struct DroneRealtimeData
    {
        public readonly float Height;
        public readonly float Speed;
        public readonly DroneInputState Input;

        public DroneRealtimeData(float height, float speed, DroneInputState input)
        {
            Height = height;
            Speed = speed;
            Input = input;
        }
    }
}
