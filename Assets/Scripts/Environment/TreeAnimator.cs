using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(DamageableObject))]
    internal class TreeAnimator : MonoBehaviour
    {
        [SerializeField] private float _duration = 2.5f;
        [SerializeField] private float _delay = 1;

        private void Start()
        {
            GetComponent<DamageableObject>().OnDestroyed += () =>
            {
                DOTween.Sequence()
                .AppendInterval(_delay)
                .Append(transform.DORotate(
                    new Vector3(Random.Range(0, 360), Random.Range(0, 360), 90), _duration))
                .AppendInterval(_duration * 2)
                .Append(transform.DOMoveY(transform.position.y - 10, _duration * 2)
                .SetEase(Ease.InCubic));
         
                Destroy(gameObject, _duration * 15);
            };
        }
    }
}