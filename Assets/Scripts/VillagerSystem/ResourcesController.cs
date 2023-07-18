using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class ResourcesController : MonoBehaviour
    {
        private enum State 
        { 
            Idle,
            TreeMining,
            RockMining,
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

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out var hit, 1000, _mask, _triggerInteraction))
                {
                    if (hit.collider.gameObject.GetComponent<MinedResource>())
                    {
                        hit.collider.gameObject.GetComponent<MinedResource>().ShowUI();

                        Debug.Log("MINED_DETECTED");
                        UpdateState(hit.collider.gameObject.GetComponent<MinedResource>());
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _currentState = State.Idle;
            }
        }

        private void UpdateState(MinedResource selected)
        {
            switch (_currentState)
            {
                case State.TreeMining:
                    if(selected.Type == MinedResourceType.Tree)
                    {
                        Debug.Log("CRATED_TREE");
                        TasksForVillager.GetTreeTasks.Enqueue(new GetTreeTask(selected));
                    }
                    break;
                case State.RockMining:
                    if (selected.Type == MinedResourceType.Rock)
                    {
                        Debug.Log("CRATED_ROCK");
                        TasksForVillager.GetRockTasks.Enqueue(new GetRockTask(selected));
                    }
                    break;
            }
        }
    }
}