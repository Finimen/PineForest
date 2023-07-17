using Assets.Scripts.InventorySystem;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building))]
    public class FactoryBuilding : MonoBehaviour
    {
        [SerializeField] private Resources _reward;

        [SerializeField] private float _duration;

        private PlayerInventory _player;

        private void Start()
        {
            _player = FindObjectOfType<PlayerInventory>();

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
                yield return new WaitForSeconds(_duration);

                _player.ChangeResources(_reward);
            }
        }
    }
}