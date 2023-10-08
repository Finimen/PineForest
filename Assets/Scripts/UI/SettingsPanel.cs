using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private UIAnimator _quality;
        [SerializeField] private UIAnimator _audio;
        [SerializeField] private UIAnimator _game;

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