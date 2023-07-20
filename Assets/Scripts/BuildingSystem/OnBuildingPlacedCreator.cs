using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class OnBuildingPlacedCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _template;

        [SerializeField] private bool _setParent;

        private void Start()
        {
            GetComponent<Building>().OnPlaced += CreateTemplate;
        }

        private void CreateTemplate()
        {
            if (_setParent)
            {
                Instantiate(_template, transform.position, Quaternion.identity, transform);
            }
            else
            {
                Instantiate(_template, transform.position, Quaternion.identity);
            }
        }
    }
}