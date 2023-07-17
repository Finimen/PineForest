using Assets.Scripts.InventorySystem;
using System;
using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class MinedResource : MonoBehaviour
    {
        public event Action OnCollected;

        [SerializeField] private Resources _reward;

        [SerializeField] private float _strength = 1;

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

        private void Collect()
        {
            IsCollected = true;

            FindObjectOfType<PlayerInventory>().ChangeResources(_reward);

            OnCollected?.Invoke();
        }
    }
}