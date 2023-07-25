using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.FireSystems
{
    [RequireComponent(typeof(FireComponent))]
    public class FireComponentsEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _fireParticles;

        [SerializeField] private Transform _effectsParent;

        [SerializeField] private Light _fireLight;

        [SerializeField] private float _emissionMultiplier;
        [SerializeField] private float _scaleMultiplier;

        private FireComponent _fireComponent;

        private Coroutine _coroutine;

        private ParticleSystem.EmissionModule[] _emissionModules;

        private void OnEnable()
        {
            _fireComponent = GetComponent<FireComponent>();

            _fireComponent.OnFireStarted += PlayParticles;
            _fireComponent.OnFireEnded += StopParticles;

            _emissionModules = new ParticleSystem.EmissionModule[_fireParticles.Length];

            HideEffects();
        }

        private void PlayParticles()
        {
            _coroutine = StartCoroutine(Update());

            IEnumerator Update()
            {
                while (true)
                {
                    for (int i = 0; i < _emissionModules.Length; i++)
                    {
                        var emissionIntensity = _fireComponent.FireIntensity * _emissionMultiplier;
                        _emissionModules[i].rateOverTime = emissionIntensity;

                        _fireLight.intensity = emissionIntensity;

                        var scaleIntensity = _fireComponent.FireIntensity * _scaleMultiplier;
                        _effectsParent.localScale = new Vector3(scaleIntensity, scaleIntensity, scaleIntensity);
                    }

                    yield return null;
                }
            }
        }

        private void StopParticles()
        {
            StopCoroutine(_coroutine);

            HideEffects();
        }

        private void HideEffects()
        {
            for (int i = 0; i < _emissionModules.Length; i++)
            {
                _emissionModules[i] = _fireParticles[i].emission;
                _emissionModules[i].rateOverTime = 0;
            }

            _effectsParent.DOScale(Vector3.zero, 5);

            _fireLight.DOIntensity(0, 5);
        }
    }
}