using Assets.Scripts.InventorySystem;
using Assets.Scripts.VillagerSystem;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private LayerMask _mask;
        [SerializeField] private QueryTriggerInteraction _triggerInteraction;

        [SerializeField] private Transform _buildingsParent;

        [SerializeField] private float _lerpSpeed = 1;

        [SerializeField] private float _cellSize = 1;

        private Camera _mainCamera;
        private PlayerInventory _playerInventory;

        private Building _current;

        private Vector3 _rotation;
        private Vector3 _position;

        public void PlaceBuilding(Building building)
        {
            if(_current != null)
            {
                DestroyCurrent();
            }

            _current = Instantiate(building, _buildingsParent);
            _current.Initialize(_playerInventory);
        }

        private void DestroyCurrent()
        {
            Destroy(_current.gameObject);
        }

        private void PlaceCurrent()
        {
            _playerInventory.ChangeResources(-_current.Price);

            _current.transform.position = _position;
            _current.transform.rotation = Quaternion.Euler(_rotation);

            TasksForVillager.BuildingTasks.Enqueue(new BuildingTask(_current));
            _current = null;
        }

        private void UpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _current.BuildingPossible)
            {
                PlaceCurrent();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                DestroyCurrent();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _rotation += new Vector3(0, 45, 0);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _rotation -= new Vector3(0, 45, 0);
            }
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            _playerInventory = FindObjectOfType<PlayerInventory>();
        }

        private void Update()
        {
            if(_current == null)
            {
                return;
            }

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out var hit, 100, _mask, _triggerInteraction))
            {
                _position = new Vector3(Mathf.Round(hit.point.x / _cellSize) * _cellSize, hit.point.y, Mathf.Round(hit.point.z / _cellSize) * _cellSize);
            }

            _current.transform.position = Vector3.Lerp(_current.transform.position, _position, _lerpSpeed * Time.deltaTime);
            _current.transform.rotation = Quaternion.Lerp(_current.transform.rotation, Quaternion.Euler(_rotation), _lerpSpeed * Time.deltaTime);

            UpdateInput();
        }
    }
}