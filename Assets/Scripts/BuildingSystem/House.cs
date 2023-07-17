using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class House : MonoBehaviour
    {
        [SerializeField] private int _villagers;

        private void OnEnable()
        {
            GetComponent<Building>().OnPlaced += ChangeVillagers;
        }

        private void ChangeVillagers()
        {

        }
    }
}