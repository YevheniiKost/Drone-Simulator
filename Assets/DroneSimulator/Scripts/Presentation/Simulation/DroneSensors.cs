using DroneSimulator.Data.Sensors;

using UnityEngine;

namespace DroneSimulator.Presentation.Simulation
{
    public class DroneSensors
    {
        private readonly Transform _droneTransform;
        private readonly Rigidbody _droneRigidbody;

        public DroneSensors(Transform droneTransform, Rigidbody droneRigidbody)
        {
            _droneTransform = droneTransform;
            _droneRigidbody = droneRigidbody;
        }

        public DroneSensorData ReadSensorData()
        {
            DroneSensorData data = new DroneSensorData();
            data.PitchAngle = ReadPitchAngle();
            data.RollAngle = ReadRollAngle();
            data.YawAngularVelocity = _droneRigidbody.angularVelocity.y;

            Vector3 localAngularVelocity =
                _droneTransform.InverseTransformDirection(_droneRigidbody.angularVelocity);
            data.PitchAngularVelocity = localAngularVelocity.x * Mathf.Rad2Deg;
            data.RollAngularVelocity = -localAngularVelocity.z * Mathf.Rad2Deg;

            data.Height = _droneTransform.localPosition.y;
            data.Speed = _droneRigidbody.linearVelocity.magnitude;
            return data;
        }

        private float ReadPitchAngle()
        {
            float xAngle = WrapAngleTo360(_droneTransform.eulerAngles.x);

            if (xAngle > 180f)
            {
                return -(360f - xAngle);
            }

            return xAngle;
        }

        private float ReadRollAngle()
        {
            float zAngle = WrapAngleTo360(_droneTransform.eulerAngles.z);

            float signedAngle;

            if (zAngle > 180f)
            {
                signedAngle = 360f - zAngle;
            }
            else
            {
                signedAngle = -zAngle;
            }

            return signedAngle;
        }

        private static float WrapAngleTo360(float angle)
        {
            return ((angle % 360f) + 360f) % 360f;
        }
    }
}
