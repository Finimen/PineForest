using System;
using UnityEngine;

namespace Assets.Scripts
{
    internal class DamageableObject : MonoBehaviour
    {
        public event Action OnDestroyed;

        public void Destroy()
        {
            OnDestroyed?.Invoke();
        }
    }
}