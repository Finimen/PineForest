using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.DebugTools
{
    [RequireComponent(typeof(UIAnimator))]
    public class DebugCanvas : MonoBehaviour
    {
        private UIAnimator _animator;

        private void Start ()
        {
            _animator = GetComponent<UIAnimator>();
        }

        private void Update ()
        {
            if(Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftControl))
            {
                _animator.SwapActive();
            }
        }
    }
}