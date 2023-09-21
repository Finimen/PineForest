using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable]
    public struct BuildingData
    {
        public Transform Transform;

        public string Name;

        public bool IsPlaced;
    }
}