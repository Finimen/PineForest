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

        private Tween _tween;

        private void OnEnable()
        {
            _tween = transform.DOScale(_size, _duration).SetEase(_ease);

            if(_destroyOnPlayed )
            {
                Destroy(gameObject, _duration + 1);
            }
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}