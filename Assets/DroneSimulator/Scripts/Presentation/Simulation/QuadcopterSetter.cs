using UnityEngine;

namespace DroneSimulator.Presentation.Simulation
{
    public class QuadcopterSetter : MonoBehaviour
    {
        [Header("Quadcopter settings")]
        [SerializeField]
        [Min(0f)]
        private float _mass;

        [SerializeField]
        [Min(0f)]
        private float _radius;

        [Header("Components")]
        [SerializeField]
        private Rigidbody _rigidBody;
        [SerializeField]
        private BoxCollider _boxCollider;

        [Space]
        [SerializeField]
        private Transform _propellerFrontRight;
        [SerializeField]
        private Transform _propellerFrontLeft;
        [SerializeField]
        private Transform _propellerBackRight;
        [SerializeField]
        private Transform _propellerBackLeft;

        private void OnValidate()
        {
            if (_rigidBody == null)
            {
                Debug.LogError("[QuadcopterSetter] Rigidbody reference is missing.");
                return;
            }

            if (_boxCollider == null)
            {
                Debug.LogError("[QuadcopterSetter] BoxCollider reference is missing.");
                return;
            }

            if (_propellerFrontRight == null || _propellerFrontLeft == null ||
                _propellerBackRight == null || _propellerBackLeft == null)
            {
                Debug.LogError("[QuadcopterSetter] One or more propeller references are missing.");
                return;
            }

            if (_mass <= 0)
            {
                Debug.LogError("[QuadcopterSetter] Mass must be greater than zero.");
                return;
            }

            if (_radius <= 0)
            {
                Debug.LogError("[QuadcopterSetter] Radius must be greater than zero.");
                return;
            }

            _rigidBody.mass = _mass;
            float height = _boxCollider.size.y;
            _boxCollider.size = new Vector3(_radius * 2f, height, _radius * 2f);

            _propellerFrontRight.localPosition = new Vector3(_radius, 0f, _radius);
            _propellerFrontLeft.localPosition = new Vector3(-_radius, 0f, _radius);
            _propellerBackRight.localPosition = new Vector3(_radius, 0f, -_radius);
            _propellerBackLeft.localPosition = new Vector3(-_radius, 0f, -_radius);
        }
    }
}
