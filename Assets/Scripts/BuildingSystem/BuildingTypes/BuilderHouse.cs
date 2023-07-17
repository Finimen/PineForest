using Assets.Scripts.VillagerSystem;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    internal class BuilderHouse : MonoBehaviour
    {
        [SerializeField] private int _builders;

        private Building _building;

        private void Start()
        {
            _building = GetComponent<Building>();
            _building.OnPlaced += () => FindObjectOfType<VillagerProfessionChanger>().BuildersToCreate += _builders;
        }
    }
}