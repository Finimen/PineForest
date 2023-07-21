using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class OnBuildingPlacedEnabler : MonoBehaviour
    {
        [SerializeField] private GameObject _part;

        private void Start()
        {
            _part.SetActive(false);

            if (GetComponent<Building>().IsPlaced)
            {
                _part.SetActive(true);
            }
            else
            {
                GetComponent<Building>().OnPlaced += () => _part.SetActive(true);
            }
        }
    }
}