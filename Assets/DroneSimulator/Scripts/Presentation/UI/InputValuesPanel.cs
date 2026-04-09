using DroneSimulator.Data.Input;

using UnityEngine;

namespace DroneSimulator.Presentation.UI
{
    public class InputValuesPanel : MonoBehaviour
    {
        [SerializeField]
        private DroneInputVisualizer _leftInputVisualizer;

        [SerializeField]
        private DroneInputVisualizer _rightInputVisualizer;

        public void UpdateInputValues(DroneInputState state)
        {
            float normalizedYaw = GetNormalizedToOneValue(state.Yaw);
            float normalizedPitch = GetNormalizedToOneValue(state.Pitch);
            float normalizedRoll = GetNormalizedToOneValue(state.Roll);

            _leftInputVisualizer.UpdateInput(state.Throttle, normalizedYaw);
            _rightInputVisualizer.UpdateInput(normalizedPitch, normalizedRoll);
        }

        private static float GetNormalizedToOneValue(float value)
        {
            return (value + 1f) * 0.5f;
        }
    }
}
