using System;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace Assets.Scripts.WeatherSystem
{
    [ExecuteInEditMode]
    public class ChangerDayAndNight : MonoBehaviour
    {
        [SerializeField] private Gradient _directionalLight;
        [SerializeField] private Gradient _ambientLight;
        [SerializeField] private Gradient _fog;

        [Space(25)]
        [SerializeField] private TMPro.TMP_Text _text;

        [SerializeField, Range(0.0f, 3600)] private float _timeDay = 60;
        [SerializeField, Range(0.0f, 1.0f)] private float _timeProgress;

        [SerializeField] private Light _directional;

        private Vector3 _defaultAngels;

        public float TimeDay
        {
            get
            {
                return _timeProgress;
            }
        }
        public float DayDuration
        {
            get
            {
                return _timeDay;
            }
        }

        public void SetTime(int time)
        {
            _timeProgress = Mathf.InverseLerp(0, 24, time);
        }
        public void SetDayDuration(int duration)
        {
            _timeDay = duration;
        }

        public void SetGradients(Gradient lights, Gradient fog)
        {
            _directionalLight = lights;
            _fog = fog;
        }

        private void Awake()
        {
            _defaultAngels = _directional.transform.localEulerAngles;
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                _timeProgress += Time.deltaTime / _timeDay;
            }

            if (_timeProgress > 1)
            {
                _timeProgress = 0;
            }

            _directional.color = Color.Lerp(_directional.color, _directionalLight.Evaluate(_timeProgress), .01f);

            RenderSettings.ambientLight = _ambientLight.Evaluate(_timeProgress);
            RenderSettings.skybox.color = _ambientLight.Evaluate(_timeProgress);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, _fog.Evaluate(_timeProgress), .01f);

            float hoursTime = _timeProgress * 24;
            float totalMinutes = hoursTime * 60;
            int hours = Mathf.FloorToInt(totalMinutes / 60);
            int minutes = Mathf.FloorToInt(totalMinutes % 60);

            string timeStr = string.Format("{0:00}:{1:00}", hours, minutes);

            if (_text != null)
            {
                _text.text = $"Time: {timeStr}";
            }

            _directional.transform.localEulerAngles = new Vector3(Mathf.Clamp(360f * _timeProgress - 90, 0, 180), _defaultAngels.x, _defaultAngels.y);
        }
    }
}