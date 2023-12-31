﻿using Assets.Scripts.SaveSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class MinedResource : SaveableObject
    {
        public event Action OnCollected;

        [Space(25)]
        [SerializeField] private Resources _reward;

        [SerializeField] private float _strength = 1;

        private DoScaler _targetUI;

        [field: SerializeField] public MinedResourceType Type { get; private set; }

        [field: SerializeField] public bool IsCollected { get; private set; }

        public void DecreaseStrength(Villager miner)
        {
            _strength -= Time.fixedDeltaTime * World.WorkEfficiency;

            if(_strength <= 0)
            {
                Collect(miner);
            }
        }

        public void ShowUI()
        {
            _targetUI.SetScale(Vector3.one);
        }

        public void HideUI()
        {
            _targetUI.SetScale(Vector3.zero);
        }

        private void Collect(Villager miner)
        {
            IsCollected = true;

            miner.GiveResources(_reward);

            HideUI();

            GetComponent<DamageableObject>()?.Destroy();

            OnCollected?.Invoke();
        }

        private void Start()
        {
            _targetUI = GetComponentInChildren<DoScaler>();
        }
    }
}