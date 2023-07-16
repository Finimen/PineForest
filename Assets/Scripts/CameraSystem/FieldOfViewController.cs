using UnityEngine;

namespace Assets.Scripts.CameraSystem
{
    [RequireComponent(typeof(Camera))]
    public class FieldOfViewController : MonoBehaviour
    {
        [SerializeField] private float _minField = 1f;
        [SerializeField] private float _maxField = 2f;

        [SerializeField] private float _multiplier = 1;

        private Camera _camera;

        private float _field = 0f;

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();

            _field = _camera.fieldOfView;
        }

        private void Update()
        {
            _field = Mathf.Clamp(_field + Input.mouseScrollDelta.y * _multiplier, _minField, _maxField);

            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _field - Input.mouseScrollDelta.y * _multiplier, 5 * Time.deltaTime);
        }
    }
}