using Assets.Scripts.VillagerSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable]
    internal struct VillagerData
    {
        public Transform Transform;

        public Villager.ProfessionType ProfessionType;
    }
}