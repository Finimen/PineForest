using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class MinedResource : MonoBehaviour
    {
        [SerializeField] private Resources _reward;

        [SerializeField] private float _strength = 1;

        [field: SerializeField] public MinedResourceType Type { get; private set; }
        
        public void DecreaseStrength()
        {
            _strength -= Time.fixedDeltaTime;
        }
    }
}