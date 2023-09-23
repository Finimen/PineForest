using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable]
    public struct BuildingData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public string Name;

        public bool IsPlaced;
    }
}