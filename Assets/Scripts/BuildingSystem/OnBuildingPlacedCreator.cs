using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class OnBuildingPlacedCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _template;

        private void Start()
        {
            GetComponent<Building>().OnPlaced += CreateTemplate;
        }

        private void CreateTemplate()
        {
            Instantiate(_template, transform.position, Quaternion.identity);
        }
    }
}