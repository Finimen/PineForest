﻿using System;
using UnityEngine;

namespace Assets.Scripts.TranslatorSystem
{
    [Serializable]
    public class Sentence
    {
        [field: SerializeField] public Language Language { get; private set; }
        [field: SerializeField, TextArea] public string Text { get; private set; }

        public void SetText(string text)
        {
            Text = text;
        }
    }
}