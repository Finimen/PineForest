using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIAnimator : MonoBehaviour
    {
        [SerializeField] private float _duration = .25f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        [SerializeField] private bool _active;

        public void SetActive(bool active)
        {
            _active = active;

            UpdateScale();
        }

        public void SwapActive()
        {
            SetActive(!_active);
        }

        private void UpdateScale()
        {
            transform.DOScale(_active ? Vector3.one : Vector3.zero, _duration).SetEase(_ease);
        }

        private void OnEnable()
        {
            SetActive(_active);
        }
    }
}