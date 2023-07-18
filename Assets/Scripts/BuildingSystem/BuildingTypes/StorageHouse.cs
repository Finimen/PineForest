using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    public class StorageHouse : MonoBehaviour
    {
        [SerializeField] private Resources _resources;

        public Resources Resources => _resources;

        public void TransferResources(Resources resources)
        {
            _resources += resources;
        }

        private void OnEnable()
        {
            World.Storages.Add(this);
        }

        private void OnDisable()
        {
            World.Storages.Remove(this);
        }
    }
}