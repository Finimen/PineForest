using UnityEngine;

namespace Assets.Scripts.WeatherSystem
{
    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private Weather _weather;

        [SerializeField] private TMPro.TMP_Text _weatherText;
        [SerializeField] private TMPro.TMP_Text _sunText;
        [SerializeField] private TMPro.TMP_Text _windText;

        [Space(25)]
        [SerializeField] private WeatherData _sunny;
        [SerializeField] private WeatherData _rainy;
        [SerializeField] private WeatherData _stormy;
        [SerializeField] private WeatherData _clean;

        private ChangerDayAndNight _changerDayAndNight;

        private WeatherData _current;

        public float SunEfficinty {get; private set;}
        public float WindEfficinty { get; private set; }

        public void SetRainy()
        {
            SetWeather(Weather.Rainy);
        }

        public void SetSunny()
        {
            SetWeather(Weather.Sunny);
        }

        public void SetWeather(Weather weather)
        {
            this._weather = weather;

            switch (this._weather)
            {
                case Weather.Rainy:
                    _current = _rainy;
                    break;
                    case Weather.Sunny:
                    _current = _sunny;
                    break;
                case Weather.Storm:
                    _current = _stormy;
                    break;
                case Weather.Clean:
                    _current = _clean;
                    break;
            }
        }

        private void Awake()
        {
            _changerDayAndNight = FindObjectOfType<ChangerDayAndNight>();
            SetWeather(Weather.Clean);
        }

        private void Update()
        {
            UpdateWeather();

            _weatherText.text = $"{_weather}";
            _sunText.text = $"{SunEfficinty * 100}%";
            _windText.text = $"{WindEfficinty * 100}%";
        }

        private void UpdateWeather()
        {
            DisableAnyWeathers();

            if(_current.Particles != null)
            {
                _current.Particles.enableEmission = true;
            }
            if (_current.Audio != null && !_current.Audio.isPlaying)
            {
                _current.Audio.Play();
            }

            _changerDayAndNight.SetGradients(_current.Light, _current.Fog);
            RenderSettings.fogDensity = _current.FogAmount;

            SunEfficinty = _current.SunEfficiency;
            WindEfficinty = _current.WindEfficiency;
        }

        private void DisableAnyWeathers()
        {
            if(_weather != Weather.Rainy)
            {
                _rainy.Particles.enableEmission = false;
                _rainy.Audio.Stop();
            }

            if(_weather != Weather.Storm)
            {
                _stormy.Particles.enableEmission = false;
                _stormy.Audio.Stop();
            }

            if(_weather != Weather.Sunny)
            {
                _sunny.Particles.enableEmission = false;
                _sunny.Audio.Stop();
            }
        }
    }
}