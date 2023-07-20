using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(DoScaler))]
    public class RandomPart : MonoBehaviour
    {
        private DoScaler scaler;

        public void Show()
        {
            scaler.SetScale(Vector3.one);
        }

        public void Hide()
        {
            scaler.SetScale(Vector3.zero);
        }

        private void Start()
        {
            scaler = GetComponent<DoScaler>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Default")) 
            {
                Hide();
            }
        }
    }
}