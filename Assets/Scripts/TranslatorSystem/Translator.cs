using System;
using UnityEngine;

namespace Assets.Scripts.TranslatorSystem
{
    public class Translator : MonoBehaviour
    {
        [field: SerializeField] public Language Current { get; private set; }

        [SerializeField, Space(25)] private KeySentence[] keySentences;

        public event Action<Language> OnLanguageChanged;

        public void SetLanguage(Language language)
        {
            Current = language;

            OnLanguageChanged?.Invoke(language);
        }

        public string Translate(string sentence)
        {
            if (Current == Language.English)
            {
                return sentence;
            }
            else
            {
                foreach (var keySentence in keySentences)
                {
                    if (keySentence.Key == sentence)
                    {
                        foreach (var result in keySentence.Sentences)
                        {
                            if (result.Language == Current)
                            {
                                return result.Text;
                            }
                        }
                    }
                }
            }

            Debug.Log($"Translation not found {sentence}");
            return sentence;
        }

        private void OnEnable()
        {
            Current = (Language)PlayerPrefs.GetInt("Language");
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("Language", (int)Current);
        }

        [Serializable]
        public class KeySentence
        {
            [field: SerializeField] public string Key { get; private set; }

            [field: SerializeField] public Sentence[] Sentences { get; private set; }
        }
    }
}