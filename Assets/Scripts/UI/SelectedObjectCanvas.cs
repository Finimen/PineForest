using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI 
{
    public class SelectedObjectCanvas : MonoBehaviour
    {
        [SerializeField] private DoScaler _panel;

        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _description;

        public void SetText(string header, string description)
        {
            _header.text = header;
            _description.text = description;
        }

        public void ShowUI()
        {
            _panel.SetScale(Vector3.one);
        }

        public void HideUI()
        {
            _panel.SetScale(Vector3.zero);
        }
    }
}