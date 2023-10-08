using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ArrayScaleController : MonoBehaviour
    {
        [SerializeField] private UIAnimator[] _controllers;

        [SerializeField] private bool _getInChildren;

        public void SetActive(bool active)
        {
            foreach (var controller in _controllers)
            {
                controller.SetActive(active);
            }
        }

        private void OnEnable()
        {
            if(_getInChildren )
            {
                _controllers = GetComponentsInChildren<UIAnimator>();
            }
        }
    }
}