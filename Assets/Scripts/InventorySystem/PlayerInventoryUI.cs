using TMPro;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInventoryUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _info;

        private PlayerInventory inventory;

        private void Start()
        {
            inventory = GetComponent<PlayerInventory>();
        }

        private void Update()
        {
            _info.text = inventory.Resources.ToString();
        }
    }
}