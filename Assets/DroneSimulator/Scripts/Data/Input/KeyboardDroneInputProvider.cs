using UnityEngine;

namespace DroneSimulator.Data.Input
{
    public class KeyboardDroneInputProvider : MonoBehaviour, IDroneInputProvider
    {
        public DroneInputState ReadInput()
        {
            DroneInputState state = new DroneInputState();

            if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                state.Throttle = 1f;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                state.Throttle = -1f;
            }

            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                state.Pitch = 1f;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                state.Pitch = -1f;
            }

            if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                state.Roll = 1f;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                state.Roll = -1f;
            }

            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                state.Yaw = 1f;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                state.Yaw = -1f;
            }

            return state;
        }
    }
}
