using Assets.Scripts.VillagerSystem;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Resources _resources;

        [SerializeField] private int _unemployed;
        [SerializeField] private int _builder;
        [SerializeField] private int _logger;
        [SerializeField] private int _mason;

        [SerializeField] private int _villagers;

        public Resources Resources => _resources;
        public int Unemployed => _unemployed;
        public int Builder => _builder;
        public int Logger => _logger;
        public int Mason => _mason;

        public int Villagers => _villagers;

        private void Update()
        {
            _villagers = World.Villagers.Count;

            _unemployed = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.None).Count;
            _builder = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Builder).Count;
            _logger = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Logger).Count;
            _mason = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Mason).Count;

            _resources = Resources.Empty;

            foreach (var storage in World.Storages)
            {
                _resources += storage.Resources;
            }
        }
    }
}