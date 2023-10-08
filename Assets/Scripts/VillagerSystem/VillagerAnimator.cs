using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    [RequireComponent(typeof(Villager))]
    public class VillagerAnimator : MonoBehaviour
    {
        [SerializeField] private string _walkName = "Walk";
        [SerializeField] private string _runName = "Run";
        [SerializeField] private string _buildingName = "Building";
        [SerializeField] private string _miningName = "Mining";
        [SerializeField] private string _pickupName = "PickUp";
        [SerializeField] private string _deathName = "Death";

        private Villager _controller;
        private Animator _animator;

        private Vector3 _lastPosition;

        private void Start()
        {
            _controller  = GetComponent<Villager>();

            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if(transform.position != _lastPosition)
            {
                SetAnimation(_walkName);
            }
            else
            {
                if (_controller.IsMining)
                {
                    SetAnimation(_miningName);
                }
                else if (_controller.IsGetting)
                {
                    SetAnimation(_pickupName);
                }
                else if (_controller.IsBuilding)
                {
                    SetAnimation(_buildingName);
                }
                else
                {
                    SetAnimation("");
                }
            }

            _lastPosition = transform.position;
        }

        private void SetAnimation(string name)
        {
            _animator.SetBool(_walkName, _walkName == name);
            _animator.SetBool(_runName, _runName == name);
            _animator.SetBool(_miningName, _miningName == name);
            _animator.SetBool(_pickupName, _pickupName == name);
            _animator.SetBool(_deathName, _deathName == name);
        }
    }
}