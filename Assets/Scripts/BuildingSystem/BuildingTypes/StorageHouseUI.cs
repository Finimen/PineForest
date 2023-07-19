using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(StorageHouse), typeof(SelectableObjectUI))]
    public class StorageHouseUI : MonoBehaviour
    {
        private StorageHouse _storage;
        private SelectableObjectUI _selectableUI;

        private void Start()
        {
            _storage = GetComponent<StorageHouse>();
            _selectableUI = GetComponent<SelectableObjectUI>();
        }

        private void Update()
        {
            _selectableUI.SetDescription($"Stored resources:\n{_storage.Resources}");
        }
    }
}