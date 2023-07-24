﻿using UnityEngine;

namespace Assets.Scripts.TranslatorSystem
{
    [RequireComponent(typeof(TMPro.TMP_Text)), ExecuteInEditMode]
    public class TextTranslator : MonoBehaviour
    {
        [SerializeField] private Sentence[] sentences;

        private TMPro.TMP_Text text;

        private void Start()
        {
            text = GetComponent<TMPro.TMP_Text>();

            FindObjectOfType<Translator>().OnLanguageChanged += SetText;
            SetText(FindObjectOfType<Translator>().Current);
        }

        private void SetText(Language language)
        {
            foreach (var sentence in sentences)
            {
                if (sentence.Language == language)
                {
                    text.text = sentence.Text;
                    break;
                }
            }
        }

        [ContextMenu("SetEnglishText")]
        private void SetEnglishText()
        {
            foreach (var sentence in sentences)
            {
                if (sentence.Language == Language.English)
                {
                    sentence.SetText(text.text);
                }
            }
        }
    }
}