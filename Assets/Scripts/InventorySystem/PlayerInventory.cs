using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Resources _resources;

        public Resources Resources => _resources;

        public void DecreaseResources(Resources resources)
        {
            _resources -= resources;
        }
    }
}