namespace DroneSimulator.Domain
{
    public readonly struct PidLastCompute
    {
        public readonly float P;
        public readonly float I;
        public readonly float D;
        public readonly float Total;

        public PidLastCompute(float p, float i, float d, float total)
        {
            P = p;
            I = i;
            D = d;
            Total = total;
        }
    }
}
