using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class StorageHouse : MonoBehaviour
    {
        [SerializeField] private Resources _resources;

        [SerializeField] private int _maxResources = 100;

        private Building _building;

        public Building Building => _building;

        public Resources Resources => _resources;
        public int MaxResources => _maxResources;

        public void TransferResources(Resources resources)
        {
            _resources += resources;

            if(resources.TotalCount() > _maxResources)
            {
                throw new System.InvalidOperationException();
            }
        }

        public void SetResources(Resources resources)
        {
            _resources = resources;
        }

        private void Awake()
        {
            _building = GetComponent<Building>();
        }

        private void Start()
        {
            if (_building.IsPlaced)
            {
                World.Storages.Add(this);
            }
            else
            {
                _building.OnPlaced += () => World.Storages.Add(this);
            }
        }

        private void OnDisable()
        {
            World.Storages.Remove(this);
        }
    }
}