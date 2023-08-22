using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class DoScaler : MonoBehaviour
    {
        [SerializeField] private Vector3 _size;

        [SerializeField] private float _duration;

        [SerializeField] private Ease _ease;

        [SerializeField] private bool _destroyOnPlayed;

        [SerializeField] private bool _playOnAwake = true;

        [SerializeField] private bool _onEnableImmediately = true; //DevHrytsan: Added little checkbox. If scene only started scale will be immediately set.

        private Tween _tween;

        public void SetDestroyOnPlayed(bool destroy)
        {
            _destroyOnPlayed = destroy;
        }

        public void SetScale(Vector3 size)
        {
            SetScale(size, _duration);
        }

        public void SetScale(Vector3 size, float duration)
        {
            _size = size;

            _tween = transform.DOScale(_size, duration).SetEase(_ease);

            if (_destroyOnPlayed)
            {
                Destroy(gameObject, duration + 1);
            }
        }

        private void OnEnable()
        {
            if (_playOnAwake)
            {
                if (_onEnableImmediately)
                {
                    SetScale(_size, 0);
                }
                else
                {
                    SetScale(_size, _duration);
                }
            }

        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}