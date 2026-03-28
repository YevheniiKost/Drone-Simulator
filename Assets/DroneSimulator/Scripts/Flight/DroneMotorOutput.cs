namespace DroneSimulator.Flight
{
    public struct DroneMotorOutput
    {
        public float FrontRightForce;
        public float FrontLeftForce;
        public float BackRightForce;
        public float BackLeftForce;

        // Combined yaw torque to apply around the world up axis.
        public float YawTorque;
    }
}

