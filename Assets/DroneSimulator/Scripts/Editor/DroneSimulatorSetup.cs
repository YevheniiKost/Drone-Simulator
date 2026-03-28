using UnityEditor;
using UnityEngine;

using DroneSimulator.Config;

namespace DroneSimulator.Editor
{
    // Runs once when Unity finishes loading. Creates the default ScriptableObject
    // assets under Resources/DroneSimulator/ if they are not already present.
    [InitializeOnLoad]
    public static class DroneSimulatorSetup
    {
        private const string PhysicsConfigPath = "Assets/DroneSimulator/Resources/Config/DronePhysicsConfig.asset";
        private const string PidConfigPath = "Assets/DroneSimulator/Resources/Config/DronePIDConfig.asset";

        static DroneSimulatorSetup()
        {
            EditorApplication.delayCall += CreateDefaultAssets;
        }

        private static void CreateDefaultAssets()
        {
            bool createdAny = false;

            if (!AssetDatabase.AssetPathExists(PhysicsConfigPath))
            {
                DronePhysicsConfig physicsConfig = ScriptableObject.CreateInstance<DronePhysicsConfig>();
                AssetDatabase.CreateAsset(physicsConfig, PhysicsConfigPath);
                Debug.Log("[DroneSimulator] Created default DronePhysicsConfig at " + PhysicsConfigPath);
                createdAny = true;
            }

            if (!AssetDatabase.AssetPathExists(PidConfigPath))
            {
                DronePidConfig pidConfig = ScriptableObject.CreateInstance<DronePidConfig>();
                AssetDatabase.CreateAsset(pidConfig, PidConfigPath);
                Debug.Log("[DroneSimulator] Created default DronePIDConfig at " + PidConfigPath);
                createdAny = true;
            }

            if (createdAny)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}

