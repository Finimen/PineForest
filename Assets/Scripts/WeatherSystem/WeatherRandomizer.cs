using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.WeatherSystem
{
    public class WeatherRandomizer : MonoBehaviour
    {
        [SerializeField] private float _delay = 15;
        [SerializeField] private float _randomness = 2f;

        private WeatherSystem _weatherSystem;
        private Rigidbody rigidbody;

        private void Start()
        {
            _weatherSystem = GetComponent<WeatherSystem>();

            StartCoroutine(UpdateWeather());
        }

        private IEnumerator UpdateWeather()
        {
            yield return new WaitForSeconds(_delay * UnityEngine.Random.Range(1 / _randomness, _randomness));

            var index = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Weather)).Length);

            _weatherSystem.SetWeather(UnityEngine.Random.Range(0, _weatherSystem.WeathersCount));

            StartCoroutine(UpdateWeather());
        }
    }
}