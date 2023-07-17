using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public class WorldUI : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;

        private Camera _camera;

        private void OnEnable()
        {
            _camera = FindObjectOfType<Camera>();

            GetComponent<Canvas>().worldCamera = _camera;
        }

        private void Update()
        {
            transform.LookAt(_camera.transform);

            transform.Rotate(_offset);
        }
    }
}