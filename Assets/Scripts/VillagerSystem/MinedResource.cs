using Assets.Scripts.InventorySystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.VillagerSystem
{
    public class MinedResource : MonoBehaviour
    {
        public event Action OnCollected;

        [SerializeField] private Resources _reward;

        [SerializeField] private float _strength = 1;

        private DoScaler _targetUI;

        [field: SerializeField] public MinedResourceType Type { get; private set; }

        [field: SerializeField] public bool IsCollected { get; private set; }

        public void DecreaseStrength()
        {
            _strength -= Time.fixedDeltaTime;

            if(_strength <= 0)
            {
                Collect();
            }
        }

        public void ShowUI()
        {
            _targetUI.SetScale(Vector3.one);
        }

        public void HideUI()
        {
            _targetUI.SetScale(Vector3.zero);
        }

        private void Collect()
        {
            IsCollected = true;

            FindObjectOfType<PlayerInventory>().ChangeResources(_reward);

            HideUI();

            OnCollected?.Invoke();
        }

        private void Start()
        {
            _targetUI = GetComponentInChildren<DoScaler>();
        }
    }
}