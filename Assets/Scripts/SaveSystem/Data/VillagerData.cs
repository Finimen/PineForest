using Assets.Scripts.VillagerSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable]
    internal struct VillagerData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public Villager.ProfessionType ProfessionType;
    }
}