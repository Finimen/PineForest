using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class StorageHouse : MonoBehaviour
    {
        [SerializeField] private Resources _resources;

        private Building _building;

        public Resources Resources => _resources;

        public void TransferResources(Resources resources)
        {
            _resources += resources;
        }

        private void Start()
        {
            _building = GetComponent<Building>();

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