using Assets.Scripts.CameraSystem;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private ScaleController _menu;

        [SerializeField] private PlayerCamera _playerCamera;

        [SerializeField] private KeyCode _switchKey;

        private bool _enabled;

        public void Hide(bool changeInput = true)
        {
            _enabled = false;

            UpdateLogic(changeInput);
        }

        public void Show()
        {
            _enabled = true;

            UpdateLogic();
        }

        private void UpdateLogic(bool changeInput = true)
        {
            _menu.SetActive(_enabled);

            if (changeInput)
            {
                _playerCamera.EnableInput = !_enabled;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(_switchKey))
            {
                _enabled = !_enabled;

                UpdateLogic();
            }
        }
    }
}