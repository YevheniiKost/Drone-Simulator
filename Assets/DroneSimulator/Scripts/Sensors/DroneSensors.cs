using UnityEngine;

namespace DroneSimulator.Sensors
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
            return data;
        }

        // Unity stores eulerAngles.x in the [0, 360) range.
        // Values in [0, 180] mean the nose is tilting forward (positive pitch).
        // Values in (180, 360) mean the nose is tilting backward, so we remap
        // to a negative angle so that 0 = level and the sign tells the direction.
        private float ReadPitchAngle()
        {
            float xAngle = WrapAngleTo360(_droneTransform.eulerAngles.x);

            if (xAngle > 180f)
            {
                return -(360f - xAngle);
            }

            return xAngle;
        }

        // Same remapping as pitch, but for the Z axis (roll).
        // Unity reports increasing Z angles when rolling to the left, so we flip
        // the sign at the end so that positive = rolling right, negative = rolling left.
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

        // Normalises any angle into the [0, 360) range.
        private static float WrapAngleTo360(float angle)
        {
            return ((angle % 360f) + 360f) % 360f;
        }
    }
}

