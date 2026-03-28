using UnityEngine;

namespace DroneSimulator.Config
{
    [CreateAssetMenu(fileName = "DronePhysicsConfig", menuName = "DroneSimulator/Drone Physics Config")]
    public class DronePhysicsConfig : ScriptableObject
    {
        [Header("Motor")]
        public float MaxPropellerForce = 100f;
        public float MaxTorque = 1f;
        public float MoveFactor = 5f;

        [Header("Throttle")]
        public float ThrottleMax = 200f;
        public float ThrottleStep = 5f;

        [Header("Wind")]
        public float WindForce = 0f;
        public float WindDirection = 0f;
    }
}

