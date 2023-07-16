using Assets.Scripts.InventorySystem;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Collider))]
    public class Building : MonoBehaviour
    {
        [SerializeField] private LayerMask _objectLayer;

        [Space(25)]
        [SerializeField] private Material _ableToBuild;
        [SerializeField] private Material _unableToBuild;

        [Space(25)]
        [SerializeField] private Collider _triggerCollider;

        [Space(25)]
        [SerializeField] private Resources _price;

        private PlayerInventory _playerInventory;

        private MeshRenderer _renderer;

        private Material _default;

        private int _entryCount;

        private bool _isPaced;

        public Resources Price => _price;
        public bool BuildingPossible { get; private set; } = true;

        public void Initialize(PlayerInventory inventory)
        {
            _playerInventory = inventory;

            _renderer = GetComponent<MeshRenderer>();

            _default = _renderer.material;

            _triggerCollider.isTrigger = true;

            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                if(_triggerCollider != collider)
                {
                    collider.enabled = false;
                }
            }
        }

        public void Place()
        {
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                if (_triggerCollider != collider)
                {
                    collider.enabled = true;
                }
            }

            _triggerCollider.isTrigger = false;

            _renderer.material = _default;

            _isPaced = true;
        }

        private void Update()
        {
            if(!_isPaced)
            {
                BuildingPossible = _entryCount == 0 && _playerInventory.Resources >= Price;

                _renderer.material = BuildingPossible? _ableToBuild: _unableToBuild;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                _entryCount++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                _entryCount--;
            }
        }
    }
}