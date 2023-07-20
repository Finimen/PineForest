using UnityEngine;

namespace Assets.Scripts.WeatherSystem
{
    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _weatherText;
        [SerializeField] private TMPro.TMP_Text _sunText;
        [SerializeField] private TMPro.TMP_Text _windText;

        [Space(25)]
        [SerializeField] private int _startId;
        [SerializeField] private WeatherData[] _weathers;

        private ChangerDayAndNight _changerDayAndNight;

        private WeatherData _current;

        public int WeathersCount => _weathers.Length;

        public float SunEfficiency {get; private set;}
        public float WindEfficiency { get; private set; }

        public void SetWeather(int id)
        {
            DisableAnyWeathers();

            _current = _weathers[id];
        }

        private void UpdateWeather()
        {
            if (_current.Particles != null)
            {
                _current.Particles.enableEmission = true;
            }
            if (_current.Audio != null && !_current.Audio.isPlaying)
            {
                _current.Audio.Play();
            }

            _changerDayAndNight.SetGradients(_current.Light, _current.Fog);
            RenderSettings.fogDensity = _current.FogAmount;

            SunEfficiency = _current.SunEfficiency;
            WindEfficiency = _current.WindEfficiency;
        }

        private void DisableAnyWeathers()
        {
            foreach (var weather in _weathers)
            {
                if(weather.Particles != null)
                {
                    weather.Particles.enableEmission = false;
                }
                if(weather.Audio != null)
                {
                    weather.Audio.Stop();
                }
            }
        }

        private void Start()
        {
            _changerDayAndNight = FindObjectOfType<ChangerDayAndNight>();
            SetWeather(_startId);
        }

        private void Update()
        {
            UpdateWeather();

            _weatherText.text = $"{_current.Name}";
            _sunText.text = $"{SunEfficiency * 100}%";
            _windText.text = $"{WindEfficiency * 100}%";
        }
    }
}