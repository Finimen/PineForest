using Assets.Scripts.InventorySystem;
using Assets.Scripts.VillagerSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Collider))]
    public class Building : MonoBehaviour
    {
        public event Action OnPlaced;

        [SerializeField] private Collider _triggerCollider;

        [Space(25)]
        [SerializeField] private Resources _price;
        [SerializeField] private int _residents;
        [SerializeField] private int _needUnemployed;

        [Space(25)]
        [SerializeField] private bool _isPaced;
        [SerializeField] private float _buildingTime = 1;

        private PlayerInventory _playerInventory;

        private BuildingPart[] _parts;

        private Material _default;

        private float _buildingProgress;
        private int _entryCount;

        public Resources Price => _price;
        public bool BuildingPossible { get; private set; } = true;
        public bool IsPlaced => _isPaced;

        public void Initialize(PlayerInventory inventory)
        {
            _playerInventory = inventory;

            _parts = GetComponentsInChildren<BuildingPart>();

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

            foreach (var part in _parts)
            {
                part.UpdateMaterial(BuildingPart.BuildAbleType.Placed);
            }

            _isPaced = true;

            OnPlaced?.Invoke();

            if(_residents > 0)
            {
                FindObjectOfType<VillagerCreator>().CreateVillagers(transform.position, _residents);
            }
        }

        public void IncreaseBuildingProgress()
        {
            _buildingProgress += Time.fixedDeltaTime;

            if (_buildingProgress > _buildingTime)
            {
                Place();
            }
        }

        private void Update()
        {
            if(!_isPaced)
            {
                BuildingPossible = _entryCount == 0 && _playerInventory.Resources >= Price && _playerInventory.Unemployed >= _needUnemployed;

                foreach (var part in _parts)
                {
                    part.UpdateMaterial(BuildingPossible? BuildingPart.BuildAbleType.Able : BuildingPart.BuildAbleType.Unable);
                }
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