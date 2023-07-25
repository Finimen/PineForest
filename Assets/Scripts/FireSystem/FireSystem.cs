using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FireSystems
{
    public class FireSystem : MonoBehaviour
    {
        public static List<FireComponent> Components = new List<FireComponent>();

        [SerializeField] private float _fireUpdateTickRate = .02f;
        [SerializeField] private float _fireSpreadingTickRate = .5f;

        [SerializeField, Range(0, 1)] private float _spreadingChance = .5f;

        private void Start()
        {
            StartCoroutine(FireUpdate());
            StartCoroutine(FireSpread());
        }

        private IEnumerator FireUpdate()
        {
            while (true)
            {
                var fireComponents = Components.FindAll(x => x.IsFire == true);

                if (fireComponents.Count > 0)
                {
                    foreach (var fireComponent in fireComponents)
                    {
                        fireComponent.FireTime -= _fireUpdateTickRate;
                        fireComponent.FireIntensity += _fireUpdateTickRate - World.RainEfficiency * _fireUpdateTickRate * Random.Range(.5f, 4.5f);

                        if(fireComponent.FireIntensity < 0 || fireComponent.FireTime <= 0)
                        {
                            fireComponent.IsFire = false;
                        }
                    }
                }

                yield return new WaitForSeconds(_fireUpdateTickRate);
            }
        }

        private IEnumerator FireSpread()
        {
            while (true)
            {
                var fireComponents = Components.FindAll(x => x.IsFire == true);

                if (fireComponents.Count > 0)
                {
                    var notFireComponents = Components.FindAll(x => x.IsFire == false);

                    foreach (var fireComponent in fireComponents)
                    {
                        foreach (var notFireComponent in notFireComponents)
                        {
                            if (Vector3.Distance(fireComponent.transform.position, notFireComponent.transform.position) < fireComponent.FireSpreadRadius)
                            {
                                var chance = _spreadingChance * fireComponent.FireIntensity / Vector3.Distance(fireComponent.transform.position, notFireComponent.transform.position);
                                if (chance > Random.Range(0f, 1f))
                                {
                                    notFireComponent.IsFire = true;
                                }
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(_fireSpreadingTickRate);
            }
        }
    }
}