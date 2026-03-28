using UnityEngine;

namespace DroneSimulator.Config
{
    [CreateAssetMenu(fileName = "DronePIDConfig", menuName = "DroneSimulator/Drone PID Config")]
    public class DronePidConfig : ScriptableObject
    {
        [Header("Pitch PID  (x = P,  y = I,  z = D)")]
        public Vector3 PitchGains = new Vector3(2f, 3f, 2f);

        [Header("Roll PID  (x = P,  y = I,  z = D)")]
        public Vector3 RollGains = new Vector3(2f, 0.2f, 0.5f);

        [Header("Yaw PID  (x = P,  y = I,  z = D)")]
        public Vector3 YawGains = new Vector3(1f, 0f, 0f);

        [Header("Integral Clamp")]
        public float ErrorSumMax = 20f;
    }
}

