using UnityEngine;

namespace Assets.Scripts.CameraSystem
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float _multiplier = 1;
        [SerializeField] private float _lerpSpeed = 1;

        [SerializeField] private Transform _camera;

        private Vector3 _startPosition;

        private Vector3 _destination;

        [field: SerializeField] public bool EnabelInput { get; set; }

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
            if (EnabelInput)
            {
                ReadInput();
            }

            MoveCamera();

            if(Physics.Raycast(transform.position, Vector3.down, 7.5f))
            {
                _destination += Vector3.up / 2 * Time.deltaTime;
            }
        }

        private void ReadInput()
        {
#if UNITY_ANDROID
            if (Input.touchCount == 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startPosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(0))
                {
                    Vector3 delta = Input.mousePosition - startPosition;

                    distanation += (new Vector3((transform.forward * -delta.y).x, 0, (transform.forward * -delta.y).z) + transform.right * -delta.x) * multiplay * Mathf.Clamp(transform.position.y, 0, 95) / 100;

                    startPosition = Input.mousePosition;
                }
            }
            else if (Input.touchCount == 2)
            {
                var touchA = Input.GetTouch(0);
                var touchB = Input.GetTouch(1);

                var touchADircetion = touchA.position - touchA.deltaPosition;
                var touchBDircetion = touchB.position - touchB.deltaPosition;

                var distanceBetwenTouchesPosition = Vector2.Distance(touchA.position, touchB.position);
                var distanceBetwenTouchesDirections = Vector2.Distance(touchADircetion, touchBDircetion);

                scroll += Vector3.up * (distanceBetwenTouchesDirections - distanceBetwenTouchesPosition) * .01f;
                scroll = new Vector3(scroll.x, Mathf.Clamp(scroll.y, minScroll, maxScroll), scroll.z);
            }
#else
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
#endif
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