using System;
using UnityEngine;

namespace Assets.Scripts.WeatherSystem
{
    [Serializable]
    public class WeatherData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ParticleSystem Particles { get; private set; }
        [field: SerializeField] public AudioSource Audio { get; private set; }
        [field: SerializeField] public Gradient Light { get; private set; }
        [field: SerializeField] public Gradient Fog { get; private set; }
        [field: SerializeField] public float FogAmount { get; private set; }

        [field: Space(25)]
        [field: SerializeField] public float WorkEfficiency { get; private set; }
        [field: SerializeField] public float MaxSunIntensity { get; private set; }
        [field: SerializeField] public float RainIntensity { get; private set; }
    }
}