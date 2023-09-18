using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public class WorldUI : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;

        private Camera _camera;
        
        private Vector3 _lastPosition;

        private const float _minMoving = .1f;

        private void OnEnable()
        {
            _camera = FindObjectOfType<Camera>();

            GetComponent<Canvas>().worldCamera = _camera;
        }

        private void FixedUpdate()
        {
            if(_lastPosition.sqrMagnitude - _camera.transform.position.sqrMagnitude > _minMoving)
            {
                _lastPosition = _camera.transform.position;

                transform.LookAt(_camera.transform);

                transform.Rotate(_offset);
            }
        }
    }
}