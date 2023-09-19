using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private ScaleController _menu;

        [SerializeField] private KeyCode _switchKey;

        private bool _enabled;

        public void Hide()
        {
            _enabled = false;

            _menu.SetActive(_enabled);
        }

        public void Show()
        {
            _enabled = true;

            _menu.SetActive(_enabled);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_switchKey))
            {
                _enabled = !_enabled;

                _menu.SetActive(_enabled);
            }
        }
    }
}