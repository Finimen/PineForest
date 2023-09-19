using Assets.Scripts.TranslatorSystem;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI 
{
    public class SelectedObjectCanvas : MonoBehaviour
    {
        [SerializeField] private DoScaler _panel;

        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _description;

        private Translator _translator;

        public void SetText(string header, string description)
        {
            _header.text = _translator.Translate(header);
            _description.text = _translator.Translate(description);
        }

        public void ShowUI()
        {
            _panel.SetScale(Vector3.one);
        }

        public void HideUI()
        {
            _panel.SetScale(Vector3.zero);
        }

        private void Awake()
        {
            _translator = FindObjectOfType<Translator>();
        }
    }
}