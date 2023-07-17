using Assets.Scripts.VillagerSystem;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class BuilderHouse : MonoBehaviour
    {
        [SerializeField] private int _builders;

        private void Start()
        {
            var building = GetComponent<Building>();
            building.OnPlaced += () => FindObjectOfType<VillagerProfessionChanger>().BuildersToCreate += _builders;
        }
    }
}