using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SelectableObjectSelector : MonoBehaviour
    {
        [SerializeField] private LayerMask _mask;
        [SerializeField] private QueryTriggerInteraction _triggerInteraction;

        [SerializeField] private float _minMouseDistance;

        private Camera _mainCamera;

        private SelectableObjectUI _selected;

        private Vector3 _start;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _start = Input.mousePosition;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && Vector3.Distance(_start, Input.mousePosition) < _minMouseDistance)
            {
                _selected?.Hide();

                if (Physics.Raycast(ray, out var hit, 1000, _mask, _triggerInteraction))
                {
                    if (hit.collider.GetComponent<SelectableObjectUI>() != null)
                    {
                        _selected = hit.collider.GetComponent<SelectableObjectUI>();
                        _selected.Show();
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                _selected?.Hide();
            }
        }
    }
}