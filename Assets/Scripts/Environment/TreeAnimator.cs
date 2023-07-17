﻿using Assets.Scripts.VillagerSystem;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(MinedResource))]
    internal class TreeAnimator : MonoBehaviour
    {
        [SerializeField] private float _duration = 1;

        private void Start()
        {
            GetComponent<MinedResource>().OnCollected += () =>
            {
                DOTween.Sequence()
                .Append(transform.DORotate(
                    new Vector3(Random.Range(0, 360), Random.Range(0, 360), 90), _duration))
                .AppendInterval(_duration * 3)
                .Append(transform.DOMoveY(transform.position.y - 10, _duration * 2)
                .SetEase(Ease.InCubic));
            };
        }
    }
}