namespace DroneSimulator.Data.Input
{
    public struct DroneInputState
    {
        public float Throttle;
        public float Pitch;
        public float Roll;
        public float Yaw;

        public override string ToString()
        {
            return $"Throttle: {Throttle}, Pitch: {Pitch}, Roll: {Roll}, Yaw: {Yaw}";
        }
    }
}
