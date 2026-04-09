using UnityEngine;

namespace DroneSimulator.Data.Input
{
    public class UnityInputProvider : MonoBehaviour, IDroneInputProvider
    {
        private DroneSimulatorUnityInputActions _unityInputActions;

        private void Awake()
        {
            _unityInputActions = new DroneSimulatorUnityInputActions();
            _unityInputActions.Enable();
        }

        public DroneInputState ReadInput()
        {
            return new DroneInputState
            {
                Throttle = NormalizeToOne(),
                Yaw = _unityInputActions.Player.Yaw.ReadValue<float>(),
                Pitch = _unityInputActions.Player.Pitch.ReadValue<float>(),
                Roll = _unityInputActions.Player.Roll.ReadValue<float>(),
            };
        }

        private float NormalizeToOne()
        {
            float value = _unityInputActions.Player.Throttle.ReadValue<float>();
            return (value + 1f) * 0.5f;
        }
    }
}
