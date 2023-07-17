using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.VillagerSystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Villager : MonoBehaviour
    {
        public enum ProfessionType
        {
            None,
            Builder,
            Logger,
            Mason,
        }

        [SerializeField] private float _buildDistance = 10f;
        [SerializeField] private float _mineDistance = 3.5f;

        private NavMeshAgent _navigationController;

        private BuildingTask _buildingTask;
        private GetTreeTask _loggerTask;
        private GetRockTask _masonTask;

        [field: SerializeField] public ProfessionType Profession { get; private set; }
        public BaseVillagerTask CurrentTask { get; private set; }

        public void SetTask(BaseVillagerTask task)
        {
            CurrentTask = task;

            switch (Profession)
            {
                case ProfessionType.Builder:
                    _buildingTask = (BuildingTask)CurrentTask;
                    break;
                case ProfessionType.Logger:
                    _loggerTask = (GetTreeTask)CurrentTask;
                    break;
                case ProfessionType.Mason:
                    _masonTask = (GetRockTask)CurrentTask;
                    break;
            }
        }

        public void ChangeProfession(ProfessionType profession)
        {
            Profession = profession;
        }

        private void UpdateBuilderLogic()
        {
            if (_buildingTask.Target.IsPlaced)
            {
                CurrentTask = null;
                return;
            }

            if (Vector3.Distance(transform.position, _buildingTask.Target.transform.position) > _buildDistance)
            {
                _navigationController.stoppingDistance = _buildDistance;
                _navigationController.SetDestination(_buildingTask.Target.transform.position);
            }
            else
            {
                _buildingTask.Target.IncreaseBuildingProgress();
            }
        }

        private void UpdateMasonLogic()
        {
            if (_masonTask.Target.IsCollected)
            {
                CurrentTask = null;
                return;
            }

            if (Vector3.Distance(transform.position, _masonTask.Target.transform.position) > _mineDistance)
            {
                _navigationController.stoppingDistance = _mineDistance;
                _navigationController.SetDestination(_masonTask.Target.transform.position);
            }
            else
            {
                _masonTask.Target.DecreaseStrength();
            }
        }

        private void UpdateLoggerLogic()
        {
            if (_loggerTask.Target.IsCollected)
            {
                CurrentTask = null;
                return;
            }

            if (Vector3.Distance(transform.position, _loggerTask.Target.transform.position) > _mineDistance)
            {
                _navigationController.stoppingDistance = _mineDistance;
                _navigationController.SetDestination(_loggerTask.Target.transform.position);
            }
            else
            {
                _loggerTask.Target.DecreaseStrength();
            }
        }

        private void OnEnable()
        {
            FindObjectOfType<VillagerUpdateSystem>().Villagers.Add(this);
        }

        private void OnDisable()
        {
            if (gameObject.scene.isLoaded)
            {
                FindObjectOfType<VillagerUpdateSystem>().Villagers.Remove(this);
            }
        }

        private void Start()
        {
            _navigationController = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (CurrentTask == null)
            {
                return;
            }

            switch (Profession)
            {
                case ProfessionType.Builder:
                    UpdateBuilderLogic();
                    break;
                case ProfessionType.Logger:
                    UpdateLoggerLogic();
                    break;
                case ProfessionType.Mason:
                    UpdateMasonLogic();
                    break;
            }
        }
    }
}