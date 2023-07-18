using System;
using UnityEngine;

namespace Assets.Scripts.WeatherSystem
{
    [CreateAssetMenu(menuName = "Environment/Weather")]
    public class WeatherData : ScriptableObject
    {
        [field: SerializeField] public ParticleSystem Particles { get; private set; }
        [field: SerializeField] public AudioSource Audio { get; private set; }
        [field: SerializeField] public Gradient Light { get; private set; }
        [field: SerializeField] public Gradient Fog { get; private set; }
        [field: SerializeField] public float FogAmount { get; private set; }

        [field: Space(25)]
        [field: SerializeField] public float SunEfficiency { get; private set; }
        [field: SerializeField] public float WindEfficiency { get; private set; }
    }
}