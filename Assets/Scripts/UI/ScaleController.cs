using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ScaleController : MonoBehaviour
    {
        [SerializeField] private float _duration = .25f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        [SerializeField] private bool _active;
        [SerializeField] private bool _onEnableImmediately = true; //DevHrytsan: Added little checkbox. If scene only started scale will be immediately set.

        private Vector3 _startScale;


        public void SetActive(bool active)
        {
            _active = active;

            UpdateScale(_duration);
        }

        private void UpdateScale(float duration)
        {
            transform.DOScale(_active ? _startScale : Vector3.zero, duration).SetEase(_ease);
        }

        private void OnEnable()
        {
            _startScale = transform.localScale;

            if (_onEnableImmediately)
            {
                UpdateScale(0);
            }
            else
            {
                SetActive(_active);
            }
        }
    }
}