using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class StorageHouse : MonoBehaviour
    {
        [SerializeField] private Resources _resources;

        private Building _building;

        public Building Building => _building;

        public Resources Resources => _resources;

        public void TransferResources(Resources resources)
        {
            _resources += resources;
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

        private void OnEnable()
        {
            _building = GetComponent<Building>();
        }

        private void OnDisable()
        {
            World.Storages.Remove(this);
        }
    }
}