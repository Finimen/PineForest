using Assets.Scripts.InventorySystem;
using System;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Collider))]
    public class Building : MonoBehaviour
    {
        public event Action OnPlaced;

        [SerializeField] private LayerMask _objectLayer;

        [Space(25)]
        [SerializeField] private Material _ableToBuild;
        [SerializeField] private Material _unableToBuild;

        [Space(25)]
        [SerializeField] private Collider _triggerCollider;

        [Space(25)]
        [SerializeField] private Resources _price;

        [Space(25)]
        [SerializeField] private bool _isPaced;
        [SerializeField] private float _buildingTime = 1;

        private PlayerInventory _playerInventory;

        private MeshRenderer _renderer;

        private Material _default;

        private float _buildingProgress;
        private int _entryCount;

        public Resources Price => _price;
        public bool BuildingPossible { get; private set; } = true;
        public bool IsPlaced => _isPaced;

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

            OnPlaced?.Invoke();
        }

        public void IncreaseBuildingProgress()
        {
            _buildingProgress += Time.deltaTime;

            if (_buildingProgress > _buildingTime)
            {
                Place();
            }
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