using Assets.Scripts.BuildingSystem;
using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class VillagerController : MonoBehaviour
    {
        private enum State 
        { 
            Idle,
            TreeMining,
            RockMining,
            BuildingDestroying
        }

        [SerializeField] private LayerMask _mask;
        [SerializeField] private QueryTriggerInteraction _triggerInteraction;

        private Camera _mainCamera;

        private State _currentState;

        public void StartTreeMining()
        {
            _currentState = State.TreeMining;
        }

        public void StartRockMining()
        {
            _currentState = State.RockMining;
        }

        public void StartBuildingDestroying()
        {
            _currentState = State.BuildingDestroying;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _currentState != State.Idle)
            {
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out var hit, 1000, _mask, _triggerInteraction))
                {
                    if (_currentState != State.BuildingDestroying && hit.collider.GetComponent<MinedResource>() != null)
                    {
                        hit.collider.gameObject.GetComponent<MinedResource>().ShowUI();

                        UpdateResourcesState(hit.collider.gameObject.GetComponent<MinedResource>());
                    }
                    else if(_currentState == State.BuildingDestroying && hit.collider.GetComponent<Building>() != null)
                    {
                        UpdateBuildingState(hit.collider.GetComponent<Building>());
                    }
                    else
                    {
                        _currentState = State.Idle;
                    }
                }
                else
                {
                    _currentState = State.Idle;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _currentState = State.Idle;
            }
        }

        private void UpdateResourcesState(MinedResource selected)
        {
            switch (_currentState)
            {
                case State.TreeMining:
                    if(selected.Type == MinedResourceType.Tree)
                    {
                        TasksForVillager.GetTreeTasks.Add(new GetTreeTask(selected));
                    }
                    break;
                case State.RockMining:
                    if (selected.Type == MinedResourceType.Rock)
                    {
                        TasksForVillager.GetRockTasks.Add(new GetRockTask(selected));
                    }
                    break;
            }
        }

        private void UpdateBuildingState(Building building)
        {
            if(building.IsPlaced)
            {
                building.StartDestroying();

                TasksForVillager.DestroyBuildingTasks.Add(new DestroyBuildingTask(building));
            }
            else
            {
                Destroy(building.gameObject);
            }
        }
    }
}