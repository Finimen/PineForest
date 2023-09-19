﻿using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ScaleController : MonoBehaviour
    {
        [SerializeField] private float _duration = .25f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        [SerializeField] private bool _active;
        [SerializeField] private bool _useStartScale = true;

        private Vector3 _startScale;

        public void SetActive(bool active)
        {
            _active = active;

            UpdateScale();
        }

        private void UpdateScale()
        {
            transform.DOScale(_active ? _useStartScale ? _startScale: Vector3.one : Vector3.zero, _duration).SetEase(_ease);
        }

        private void OnEnable()
        {
            _startScale = transform.localScale;

            SetActive(_active);
        }
    }
}