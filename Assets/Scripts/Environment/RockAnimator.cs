using Assets.Scripts.VillagerSystem;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(MinedResource))]
    internal class RockAnimator : MonoBehaviour
    {
        [SerializeField] private float _duration = 1;

        private void Start()
        {
            GetComponent<MinedResource>().OnCollected += () =>
            {
                transform.DOScale(Vector3.zero, _duration).SetEase(Ease.InOutBounce);
            };
        }
    }
}