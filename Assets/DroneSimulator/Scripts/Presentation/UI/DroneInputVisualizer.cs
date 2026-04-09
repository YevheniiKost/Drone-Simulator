using UnityEngine;

namespace DroneSimulator.Presentation.UI
{
    public class DroneInputVisualizer : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rect;

        [SerializeField]
        private Transform _pointer;

        public void UpdateInput(float vertical, float horizontal)
        {
            Vector3[] corners = new Vector3[4];
            _rect.GetLocalCorners(corners);

            Vector3 bottomLeft = corners[0];
            Vector3 topLeft = corners[1];
            Vector3 topRight = corners[2];
            Vector3 bottomRight = corners[3];

            Vector3 bottomMiddle = (bottomLeft + bottomRight) / 2f;
            Vector3 topMiddle = (topLeft + topRight) / 2f;
            Vector3 leftMiddle = (bottomLeft + topLeft) / 2f;
            Vector3 rightMiddle = (bottomRight + topRight) / 2f;

            float x = Mathf.Lerp(leftMiddle.x, rightMiddle.x, horizontal);
            float y = Mathf.Lerp(bottomMiddle.y, topMiddle.y, vertical);

            _pointer.localPosition = new Vector3(x, y, 0f);
        }
    }
}
