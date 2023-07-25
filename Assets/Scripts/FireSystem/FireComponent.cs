using System;
using UnityEngine;

namespace Assets.Scripts.FireSystems
{
    public class FireComponent : MonoBehaviour
    {
        public event Action OnFireStarted;
        public event Action OnFireEnded;

        [SerializeField] private bool _isFire; 

        public bool IsFire
        {
            set
            {
                _isFire = value;

                if (_isFire)
                {
                    OnFireStarted?.Invoke();
                }
                else
                {
                    if(FireTime <= 0)
                    {
                        GetComponent<DamageableObject>()?.Destroy();
                    }

                    OnFireEnded?.Invoke();
                }
            }
            get
            {
                return _isFire;
            }
        }

        public float FireSpreadRadius = 5;

        public float FireTime = 10;

        public float FireIntensity;

        [ContextMenu("CreateFire")]
        private void CreateFire()
        {
            IsFire = true;
        }

        private void OnEnable()
        {
            FireSystem.Components.Add(this);
        }

        private void OnDisable()
        {
            FireSystem.Components.Remove(this);
        }
    }
}