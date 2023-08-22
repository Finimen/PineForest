using Assets.Scripts.InventorySystem;
using Assets.Scripts.VillagerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.BuildingSystem
{
    public class Building : MonoBehaviour
    {
        public event Action OnPlaced;

        [SerializeField] private Transform[] _navigationPoints;

        [Space(25)]
        [SerializeField] private Collider[] _triggerColliders;

        [Space(25)]
        [SerializeField] private Resources _price;

        [Space(25)]
        [SerializeField] private int _residents;
        [SerializeField] private int _needUnemployed;

        [Space(25)]
        [SerializeField] private bool _isPaced;
        [SerializeField] private float _buildingTime = 1;

        [Space(25)]
        [SerializeField] private DoScaler _destroyingUI;

        private List<string> _debugNames = new List<string>();

        private PlayerInventory _playerInventory;

        private BuildingPart[] _parts;

        private NavMeshObstacle _navigationObstacle;

        private Resources _transferred;

        private float _buildingProgress;
        private float _destroyingProgress;
        private int _entryCount;
        private bool _isPlan;
        private bool _isDestroying;

        public Resources Price => _price;
        public Resources Transferred => _transferred;
        public Resources StartTransferring { get; set; }
        public Resources Needed => _price - _transferred;

        public float BuildingProgress => _buildingProgress / _buildingTime;
        public bool BuildingPossible { get; private set; } = true;
        public bool IsPlaced => _isPaced;
        public bool IsDestroying => _isDestroying;

        public void Initialize(PlayerInventory inventory)
        {
            _playerInventory = inventory;

            _navigationObstacle = GetComponent<NavMeshObstacle>();

            if(_navigationObstacle != null)
            {
                _navigationObstacle.enabled = false;
            }

            foreach (var trigger in _triggerColliders)
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

            if (_navigationObstacle != null)
            {
                _navigationObstacle.enabled = true;
            }

            foreach (var part in _parts)
            {
                part.UpdateMaterial(BuildingPart.BuildAbleType.Placed);
            }

            _isPaced = true;

            OnPlaced?.Invoke();

            if(_residents > 0)
            {
                FindObjectOfType<VillagerCreator>()?.CreateVillagers(transform.position, _residents);
                //Dev Hrytsan: Added little check
            }
        }

        public void IncreaseBuildingProgress()
        {
            _buildingProgress += Time.fixedDeltaTime * World.WorkEfficiency;

            if (_buildingProgress > _buildingTime)
            {
                Place();
            }
        }

        public void IncreaseDestroyingProgress()
        {
            _destroyingProgress += Time.fixedDeltaTime * World.WorkEfficiency;

            if (_destroyingProgress > _buildingTime)
            {
                Destroy(gameObject);
            }
        }

        public void StartDestroying()
        {
            foreach (var part in _parts)
            {
                part.UpdateMaterial(BuildingPart.BuildAbleType.Placed);
            }

            _destroyingUI?.SetScale(Vector3.one);

            _isDestroying = true;
        }

        public void StopDestroying()
        {
            _destroyingUI?.SetScale(Vector3.zero);

            _isDestroying = false;
        }

        public Vector3 GetNearestPoint(Vector3 his)
        {
            var nearest = _navigationPoints[0];

            foreach(var point in _navigationPoints)
            {
                if(Vector3.Distance(nearest.position, his) > Vector3.Distance(point.position, his))
                {
                    nearest = point;
                }
            }

            return nearest.position;
        }

        private void Update()
        {
            if(!_isPlan && !_isPaced)
            {
                BuildingPossible = _entryCount <= 0 && _playerInventory.Resources >= Price && _playerInventory.Unemployed >= _needUnemployed;

                foreach (var part in _parts)
                {
                    part.UpdateMaterial(BuildingPossible? BuildingPart.BuildAbleType.Able : BuildingPart.BuildAbleType.Unable);
                }
            }
        }

        private void OnEnable()
        {
            _parts = GetComponentsInChildren<BuildingPart>();

            if (_isPaced)
            {
                Initialize(FindObjectOfType<PlayerInventory>());

                Place();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isPlan)
            {
                _debugNames.Add(other.name);

                _entryCount++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!_isPlan)
            {
                _debugNames.Remove(other.name);

                _entryCount--;
            }
        }
    }
}