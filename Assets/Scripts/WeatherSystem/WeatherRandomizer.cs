using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.WeatherSystem
{
    public class WeatherRandomizer : MonoBehaviour
    {
        [SerializeField] private float _delay = 15;

        private WeatherSystem _weatherSystem;

        private void Start()
        {
            _weatherSystem = GetComponent<WeatherSystem>();

            StartCoroutine(UpdateWeather());
        }

        private IEnumerator UpdateWeather()
        {
            yield return new WaitForSeconds(_delay * UnityEngine.Random.Range(.5f, 2f));

            var index = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Weather)).Length);

            Debug.Log((Weather)index);

            _weatherSystem.SetWeather(UnityEngine.Random.Range(0, _weatherSystem.WeathersCount));

            StartCoroutine(UpdateWeather());
        }
    }
}