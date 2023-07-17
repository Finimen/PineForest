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
            Farmer
        }

        [SerializeField] private float _actionRadius = 1;
        
        private NavMeshAgent _navigationController;

        private BuildingTask _buildingTask;

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

            if (Vector3.Distance(transform.position, _buildingTask.Target.transform.position) > _actionRadius)
            {
                _navigationController.SetDestination(_buildingTask.Target.transform.position);
            }
            else
            {
                _buildingTask.Target.IncreaseBuildingProgress();
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
            _navigationController.stoppingDistance = _actionRadius;
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
            }
        }
    }
}