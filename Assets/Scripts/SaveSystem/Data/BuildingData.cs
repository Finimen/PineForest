using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable]
    public struct BuildingData
    {
        public Resources StoredResources;

        public Vector3 Position;
        public Quaternion Rotation;

        public int Id;

        public bool IsPlaced;
    }
}