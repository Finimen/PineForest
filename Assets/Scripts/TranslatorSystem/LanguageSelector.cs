using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.TranslatorSystem
{
    public class LanguageSelector : MonoBehaviour
    {
        private TMP_Dropdown _dropdown;

        private Language[] _languages;

        private void Start()
        {
            _dropdown = GetComponent<TMP_Dropdown>();

            _dropdown.ClearOptions();

            _languages = Enum.GetValues(typeof(Language)) as Language[];

            var options = new System.Collections.Generic.List<TMP_Dropdown.OptionData>();

            foreach (Language language in _languages)
            {
                options.Add(new TMP_Dropdown.OptionData(language.ToString()));
            }

            _dropdown.AddOptions(options);

            _dropdown.onValueChanged.AddListener(SetLanguage);
        }

        private void SetLanguage(int id)
        {
            FindObjectOfType<Translator>().SetLanguage(_languages[id]);
        }
    }
}