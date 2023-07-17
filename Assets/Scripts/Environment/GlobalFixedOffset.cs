using UnityEngine;

namespace Assets.Scripts.Environment
{
    [ExecuteAlways]
    public class GlobalFixedOffset : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _target;

        private void Update()
        {
            transform.position = _target.position + _offset;
        }
    }
}