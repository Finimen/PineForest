using Assets.Scripts.VillagerSystem;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Resources _resources;

        [SerializeField] private int _unemployed;

        [SerializeField] private int _villagers;

        private VillagerUpdateSystem _villagerSystem;

        public Resources Resources => _resources;
        public int Unemployed => _unemployed;
        public int Villagers => _villagers;

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
            _villagers = _villagerSystem.Villagers.Count;
            _unemployed = _villagerSystem.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.None).Count;

            _resources = Resources.Empty;

            foreach (var storage in World.Storages)
            {
                _resources += storage.Resources;
            }
        }
    }
}