using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SelectableObjectUI : MonoBehaviour
    {
        [SerializeField] private string _header;
        [SerializeField, TextArea] private string _description;

        private SelectedObjectCanvas _canvas;

        private bool _isSelected;

        public string Header => _header;
        public string Description => _description;

        public void SetHeader(string text)
        {
            _header = text;
        }

        public void SetDescription(string text)
        {
            _description = text;

            if (_isSelected)
            {
                _canvas.SetText(_header, _description);
            }
        }

        public void Hide()
        {
            _canvas.HideUI();

            _isSelected = false;
        }

        public void Show()
        {
            _canvas.ShowUI();
            _canvas.SetText(_header, _description);

            _isSelected = true;
        }

        private void Start()
        {
            _canvas = FindObjectOfType<SelectedObjectCanvas>();
        }
    }
}