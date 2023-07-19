using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SelectableObjectUI : MonoBehaviour
    {
        [SerializeField] private string _header;
        [SerializeField, TextArea] private string _description;

        private SelectedObjectCanvas _canvas;

        public void SetHeader(string text)
        {
            _header = text;
        }

        public void SetDescription(string text)
        {
            _description = text;
        }

        public void Hide()
        {
            _canvas.HideUI();
        }

        public void Show()
        {
            _canvas.ShowUI();
            _canvas.SetText(_header, _description);
        }

        private void Start()
        {
            _canvas = FindObjectOfType<SelectedObjectCanvas>();
        }
    }
}