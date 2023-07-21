using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private ScaleController _quality;
        [SerializeField] private ScaleController _audio;
        [SerializeField] private ScaleController _game;

        public void ShowQuality()
        {
            _quality.SetActive(true);
            _audio.SetActive(false);
            _game.SetActive(false);
        }

        public void ShowAudio()
        {
            _audio.SetActive(true);
            _game.SetActive(false);
            _quality.SetActive(false);
        }

        public void ShowGame()
        {
            _game.SetActive(true);
            _audio.SetActive(false);
            _quality.SetActive(false);
        }
    }
}