using UnityEngine;

namespace DroneSimulator.Data.Config
{
    public sealed class ConfigProvider : IConfigProvider
    {
        private const string PhysicsConfigResourcePath = "Config/DronePhysicsConfig";
        private const string PidConfigResourcePath = "Config/DronePIDConfig";
        private const string AcroConfigResourcePath = "Config/DroneAcroConfig";

        public DronePhysicsConfig GetPhysicsConfig()
        {
            return Resources.Load<DronePhysicsConfig>(PhysicsConfigResourcePath);
        }

        public DronePidConfig GetPidConfig()
        {
            return Resources.Load<DronePidConfig>(PidConfigResourcePath);
        }

        public DroneAcroConfig GetAcroConfig()
        {
            return Resources.Load<DroneAcroConfig>(AcroConfigResourcePath);
        }
    }
}
