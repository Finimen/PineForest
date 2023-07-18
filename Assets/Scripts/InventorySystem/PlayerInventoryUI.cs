using TMPro;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInventoryUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _info;

        private PlayerInventory _inventory;

        private void Start()
        {
            _inventory = GetComponent<PlayerInventory>();
        }

        private void Update()
        {
            _info.text = 
                $"Villagers: {_inventory.Villagers}\n" +
                $"Unemployed: {_inventory.Unemployed}\n" +
                $"{_inventory.Resources}";
        }
    }
}