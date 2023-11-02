using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable]
    public struct MinedResourceData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public int Id;

        public bool IsIsCollected;
    }
}