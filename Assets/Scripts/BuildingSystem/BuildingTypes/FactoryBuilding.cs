using Assets.Scripts.VillagerSystem;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class FactoryBuilding : MonoBehaviour
    {
        [SerializeField] private Resources _reward;

        [SerializeField] private float _duration;
        [SerializeField] private bool _useSunIntensity;

        private MoveResourcesTask _moveResourcesTask;

        private Resources _produced;

        private float _progress;

        private void Start()
        {
            GetComponent<Building>().OnPlaced += StartProduction;
        }

        private void StartProduction()
        {
            StartCoroutine(Produce());
        }

        private IEnumerator Produce()
        {
            while (true)
            {
                _progress += Time.deltaTime * (_useSunIntensity? World.SunEfficiency : 1);
                yield return null;

                if(_progress >= _duration)
                {
                    _produced += _reward;

                    if (_moveResourcesTask == null)
                    {
                        _moveResourcesTask = new MoveResourcesTask(GetComponent<Building>(), _produced);

                        TasksForVillager.MoveResourcesTasks.Add(_moveResourcesTask);
                    }
                    else
                    {
                        _moveResourcesTask.Resources = _produced;
                    }
                }
            }
        }
    }
}