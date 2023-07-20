using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class RandomPartChanger : MonoBehaviour
    {
        [SerializeField] private int _maxParts = 3;
        [SerializeField] private int _minParts = 1;

        private RandomPart[] _parts;

        private void Start()
        {
            _parts = GetComponentsInChildren<RandomPart>();

            GetComponent<Building>().OnPlaced += CreateRandomParts;
        }

        private void CreateRandomParts()
        {
            var parts = Random.Range(_minParts, _maxParts);

            for (int i = 0; i < parts; i++)
            {
                var index = Random.Range(0, _parts.Length);

                _parts[index].Show();
            }
        }
    }
}