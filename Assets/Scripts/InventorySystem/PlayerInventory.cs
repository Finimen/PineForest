using Assets.Scripts.VillagerSystem;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Resources _resources;

        [SerializeField] private int _unemployed;

        private VillagerUpdateSystem _villagerSystem;

        public Resources Resources => _resources;
        public int Unemployed => _unemployed;

        public void ChangeResources(Resources resources)
        {
            _resources += resources;
        }

        private void Start()
        {
            _villagerSystem = FindObjectOfType<VillagerUpdateSystem>();
        }

        private void Update()
        {
            _unemployed = _villagerSystem.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.None).Count;
        }
    }
}