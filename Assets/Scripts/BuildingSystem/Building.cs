using Assets.Scripts.InventorySystem;
using Assets.Scripts.VillagerSystem;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Collider),typeof(NavMeshObstacle))]
    public class Building : MonoBehaviour
    {
        public event Action OnPlaced;

        [SerializeField] private Collider[] _triggerColliders;

        [Space(25)]
        [SerializeField] private Resources _price;
        private Resources _transferred;

        [Space(25)]
        [SerializeField] private int _residents;
        [SerializeField] private int _needUnemployed;

        [Space(25)]
        [SerializeField] private bool _isPaced;
        [SerializeField] private float _buildingTime = 1;

        private PlayerInventory _playerInventory;

        private BuildingPart[] _parts;

        private NavMeshObstacle _navigationObstacle;

        private bool _isPlan;
        private float _buildingProgress;
        private int _entryCount;

        public Resources Price => _price;
        public Resources Transferred => _transferred;
        public Resources Needed => _price - _transferred;

        public float BuildingProgress => _buildingProgress / _buildingTime;
        public bool BuildingPossible { get; private set; } = true;
        public bool IsPlaced => _isPaced;

        public void Initialize(PlayerInventory inventory)
        {
            _playerInventory = inventory;

            _parts = GetComponentsInChildren<BuildingPart>();

            _navigationObstacle = GetComponent<NavMeshObstacle>();

            _navigationObstacle.enabled = false;

            foreach(var trigger in _triggerColliders)
            {
                trigger.isTrigger = true;
            } 

            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                if(!_triggerColliders.Contains(collider))
                {
                    collider.enabled = false;
                }
            }
        }

        public void TransferResources(Resources resources)
        {
            _transferred += resources;
        }

        public void InstallPlan()
        {
            foreach (var part in _parts)
            {
                part.UpdateMaterial(BuildingPart.BuildAbleType.Plan);
            }

            _isPlan = true;
            
            foreach (var trigger in _triggerColliders)
            {
                trigger.isTrigger = false;
            }
        }

        public void Place()
        {
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                if (!_triggerColliders.Contains(collider))
                {
                    collider.enabled = true;
                }
            }

            _navigationObstacle.enabled = true;

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
            if(!_isPlan && !_isPaced)
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
            if(!_isPlan && other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                _entryCount++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!_isPlan && other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                _entryCount--;
            }
        }
    }
}