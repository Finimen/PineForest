using Assets.Scripts.VillagerSystem;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class MasonHouse : MonoBehaviour
    {
        [SerializeField] private int _masons;

        private void Start()
        {
            var building = GetComponent<Building>();
            building.OnPlaced += () => FindObjectOfType<VillagerProfessionChanger>().MasonsToCreate += _masons;
        }
    }
}