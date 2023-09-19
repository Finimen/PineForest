using UnityEngine;

namespace Assets.Scripts.WeatherSystem
{
    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _weatherText;
        [SerializeField] private TMPro.TMP_Text _workText;

        [Space(25)]
        [SerializeField] private int _startId;
        [SerializeField] private float _sunIntensity;
        [SerializeField] private WeatherData[] _weathers;

        private ChangerDayAndNight _changerDayAndNight;

        private WeatherData _current;

        public WeatherData Current => _current;

        public int WeathersCount => _weathers.Length;

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

            World.WorkEfficiency = _current.WorkEfficiency;
            World.RainEfficiency = _current.RainIntensity;

            World.SunEfficiency = _sunIntensity;
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

        private void UpdateVillagers()
        {
            foreach(var villager in World.Villagers)
            {
                _sunIntensity = _changerDayAndNight.CurrentTime > .5f ? 
                    Mathf.Lerp(_current.MaxSunIntensity, 0, _changerDayAndNight.CurrentTime) * 2 :
                    Mathf.Lerp(0, _current.MaxSunIntensity, _changerDayAndNight.CurrentTime) * 2;

                var intensity = _sunIntensity;

                foreach (var source in World.LightSources)
                {
                    intensity += source.Intensity / Vector3.Distance(villager.transform.position, source.transform.position) * 10;
                }

                villager.MovingEfficiency = Mathf.Clamp(intensity, .25f, 1);
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

            UpdateVillagers();

            _weatherText.text = $"Weather: {_current.Name}";
            _workText.text = $"Work: {World.WorkEfficiency * 100}%";
        }
    }
}