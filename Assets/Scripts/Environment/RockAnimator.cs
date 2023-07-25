using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(DamageableObject))]
    internal class RockAnimator : MonoBehaviour
    {
        [SerializeField] private float _duration = 1;

        private void Start()
        {
            GetComponent<DamageableObject>().OnDestroyed += () =>
            {
                transform.DOScale(Vector3.zero, _duration).SetEase(Ease.InOutBounce);
            };
        }
    }
}