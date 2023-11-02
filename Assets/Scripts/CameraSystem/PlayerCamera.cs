using UnityEngine;

namespace Assets.Scripts.CameraSystem
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float _multiplier = 1;
        [SerializeField] private float _lerpSpeed = 1;

        [SerializeField] private float _scrollMultiplier = 1;

        [Space(25)]
        [SerializeField] private Transform _min;
        [SerializeField] private Transform _max;

        [Space(25)]
        [SerializeField] private Transform _camera;

        private Vector3 _startPosition;

        private Vector3 _destination;

        [field: SerializeField] public bool EnableInput { get; set; }

        public void SetDestination(Vector3 destination)
        {
            _destination = destination;
        }

        private void Awake()
        {
            _destination = transform.position;
        }

        private void Update()
        {
            if (EnableInput)
            {
                ReadInput();
            }

            MoveCamera();

            if(Physics.Raycast(transform.position, Vector3.down, 7.5f))
            {
                _destination += Vector3.up / 2 * Time.deltaTime;
            }

            _destination = new Vector3(Mathf.Clamp(_destination.x, _min.position.x, _max.position.x),
               Mathf.Clamp(_destination.y, _min.position.y, _max.position.y),
               Mathf.Clamp(_destination.z, _min.position.z, _max.position.z));
        }

        private void ReadInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - _startPosition;

                var transfromed = _camera.TransformDirection(delta);
                _destination -= new Vector3(transfromed.x, 0, transfromed.z);

                _startPosition = Input.mousePosition;
            }

            _destination.y -= Input.mouseScrollDelta.y * _scrollMultiplier;
        }

        private void MoveCamera()
        {
            transform.position = Vector3.Lerp(transform.position, _destination, _lerpSpeed * Time.deltaTime);
        }

        private void OnEnable()
        {
            SetDestination(transform.position + Vector3.up * 15);
        }
    }
}