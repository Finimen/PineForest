using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable] internal struct TransformData
    {
        public Vector3 Position;
        public Vector3 Scale;

        public Quaternion Rotation;

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}