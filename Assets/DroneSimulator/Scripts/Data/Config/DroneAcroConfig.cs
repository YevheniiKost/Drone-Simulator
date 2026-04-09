using UnityEngine;

namespace DroneSimulator.Data.Config
{
    [CreateAssetMenu(fileName = "DroneAcroConfig", menuName = "DroneSimulator/Drone Acro Config")]
    public class DroneAcroConfig : ScriptableObject
    {
        [Header("Rate Limits (deg/s)")]
        public float MaxPitchRate = 200f;
        public float MaxRollRate = 200f;
        public float MaxYawRate = 150f;

        [Header("Pitch Rate PID  (x = P,  y = I,  z = D) — I trims steady-state rate under gravity/disturbance")]
        public Vector3 PitchRateGains = new Vector3(0.1f, 0.03f, 0f);

        [Header("Roll Rate PID  (x = P,  y = I,  z = D)")]
        public Vector3 RollRateGains = new Vector3(0.1f, 0.03f, 0f);

        [Header("Yaw Rate PID  (x = P,  y = I,  z = D)")]
        public Vector3 YawRateGains = new Vector3(0.5f, 0f, 0f);

        [Header("Integral Clamp")]
        public float RateErrorSumMax = 20f;
    }
}
