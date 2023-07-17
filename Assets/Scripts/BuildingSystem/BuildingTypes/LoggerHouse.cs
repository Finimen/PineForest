using Assets.Scripts.VillagerSystem;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class LoggerHouse : MonoBehaviour
    {
        [SerializeField] private int _loggers;

        private Building _building;

        private void Start()
        {
            _building = GetComponent<Building>();
            _building.OnPlaced += () => FindObjectOfType<VillagerProfessionChanger>().LoggersToCreate += _loggers;
        }
    }
}