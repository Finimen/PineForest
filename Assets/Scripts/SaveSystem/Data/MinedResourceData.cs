using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable]
    public struct MinedResourceData
    {
        public Transform Transform;

        public bool IsMined;
    }
}