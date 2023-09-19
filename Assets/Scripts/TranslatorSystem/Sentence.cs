using System;
using UnityEngine;

namespace Assets.Scripts.TranslatorSystem
{
    [Serializable]
    public class Sentence
    {
        public Sentence(Language language, string text)
        {
            Language = language;
            Text = text;
        }

        [field: SerializeField] public Language Language { get; private set; }
        [field: SerializeField, TextArea(5, 5)] public string Text { get; private set; }

        public void SetText(string text)
        {
            Text = text;
        }
    }
}