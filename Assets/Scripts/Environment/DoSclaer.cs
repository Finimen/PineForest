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

        private Tween _tween;

        public void SetScale(Vector3 size)
        {
            _size = size;

            _tween = transform.DOScale(_size, _duration).SetEase(_ease);

            if (_destroyOnPlayed)
            {
                Destroy(gameObject, _duration + 1);
            }
        }

        private void OnEnable()
        {
            if (_playOnAwake)
            {
                SetScale(_size);
            }
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}