namespace DroneSimulator.Sensors
{
    public struct DroneSensorData
    {
        // Signed pitch angle in degrees.
        // Positive means the drone nose is tilting forward (down).
        public float PitchAngle;

        // Signed roll angle in degrees.
        // Positive means the drone is rolling to the right.
        public float RollAngle;

        // Angular velocity around the world up axis, in radians per second.
        // Positive means rotating counter-clockwise when viewed from above.
        public float YawAngularVelocity;
    }
}

