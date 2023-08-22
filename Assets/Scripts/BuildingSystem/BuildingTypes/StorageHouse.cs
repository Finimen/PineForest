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

            if (resources.TotalCount() > _maxResources)
            {
                throw new System.InvalidOperationException();
            }
        }
  
        public void FillFull()
        {
            float division = 1 / 3;

            //DevHrytsan: Need some constant for it or variable which determinates total resource types
            int maxResourceFood = Mathf.FloorToInt(_maxResources * division);
            int maxResourceStone = Mathf.FloorToInt(_maxResources * division);
            int maxResourceWood = Mathf.FloorToInt(_maxResources * division);

            _resources = new Resources(maxResourceFood, maxResourceWood, maxResourceStone);

        }
        public void Clean()
        {
            _resources = new Resources(0, 0, 0);
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