using Assets.Scripts.BuildingSystem;
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

        public enum WorkType
        {
            None,
            IBringResources,
            IGoToTheStorage,
            TheStoragesDoNotHaveTheNecessaryResources,
            WaitingForOtherBuildersToBringResources,
        }

        [SerializeField] private float _actionDistance = 2.5f;

        [Space(25)]
        [SerializeField] private int _maxResources = 5;
        [SerializeField] private Resources _transferring;

        [Space(25)]
        [SerializeField] private float _hunger;

        private NavMeshAgent _navigationController;

        private BuildingTask _buildingTask;
        private DestroyBuildingTask _destroyingTask;
        private GetTreeTask _loggerTask;
        private GetRockTask _masonTask;
        private MoveResourcesTask _moveTask;

        private WorkType _workType;

        private float _movingSpeed;

        [field: SerializeField, Space(25)] public ProfessionType Profession { get; private set; }
        public BaseVillagerTask CurrentTask { get; private set; }
        public float MovingEfficiency { get; set; } = 1;
        public int MaxResources => _maxResources;
        public WorkType CurrentWork => _workType;

        public void SetTask(BaseVillagerTask task)
        {
            CurrentTask = task;

            if(task is MoveResourcesTask)
            {
                _moveTask = (MoveResourcesTask)task;
                return;
            }
            else
            {
                _moveTask = null;
            }

            switch (Profession)
            {
                case ProfessionType.Builder:
                    if (CurrentTask is (BuildingTask))
                    {
                        _buildingTask = (BuildingTask)CurrentTask;
                        _destroyingTask = null;
                    }
                    else
                    {
                        _destroyingTask = (DestroyBuildingTask)CurrentTask;
                        _buildingTask = null;
                    }
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

        public void GiveResources(Resources resources)
        {
            Debug.Log("Я получил ресы");

            _transferring += resources.GetClampedResources(resources, _maxResources - _transferring.TotalCount());
        }

        private void UpdateDestroyingTask()
        {
            if (_destroyingTask.Target != null)
            {
                if (Vector3.Distance(transform.position, _destroyingTask.Target.transform.position) > _actionDistance)
                {
                    _navigationController.SetDestination(_destroyingTask.Target.transform.position);
                }
                else
                {
                    _destroyingTask.Target.IncreaseDestroyingProgress();
                }
            }
            else
            {
                CurrentTask = null;
            }
        }

        private void UpdateBuildingTask()
        {
            if (_buildingTask.Target == null || _buildingTask.Target.IsPlaced)
            {
                CurrentTask = null;
                return;
            }

            if (_buildingTask.Target.Needed.TotalCount() > 0)
            {
                if (_transferring.Contains(_buildingTask.Target.Needed))
                {
                    if (Vector3.Distance(transform.position, _buildingTask.Target.GetNearestPoint(transform.position)) > _actionDistance)
                    {
                        _navigationController.stoppingDistance = _actionDistance * .75f;
                        _navigationController.SetDestination(_buildingTask.Target.GetNearestPoint(transform.position));

                        _workType = WorkType.IBringResources;
                        Debug.Log("Несу ресы");
                    }
                    else
                    {
                        Debug.Log("Передал ресы");

                        _buildingTask.Target.TransferResources(_transferring);
                        _transferring = Resources.Empty;
                    }
                }
                else
                {
                    if (_buildingTask.Target.Price == _buildingTask.Target.StartTransferring)
                    {
                        Debug.Log("Жду пока другие типы принесут ресы");

                        _workType = WorkType.WaitingForOtherBuildersToBringResources;
                        return;
                    }

                    Debug.Log("Ищю ресы на складах");

                    var suitableList = World.Storages.FindAll(x => x.Resources.Contains(_buildingTask.Target.Price - _buildingTask.Target.StartTransferring));

                    if (suitableList.Count > 0)
                    {
                        StorageHouse nearestSuitable = null;

                        foreach (var suitable in suitableList)
                        {
                            if (nearestSuitable == null || Vector3.Distance(transform.position, nearestSuitable.Building.GetNearestPoint(transform.position)) >
                                Vector3.Distance(transform.position, suitable.Building.GetNearestPoint(transform.position)))
                            {
                                nearestSuitable = suitable;
                            }
                        }

                        if (Vector3.Distance(transform.position, nearestSuitable.Building.GetNearestPoint(transform.position)) > _actionDistance)
                        {
                            Debug.Log("Иду к складу");

                            _workType = WorkType.IGoToTheStorage;

                            _navigationController.stoppingDistance = _actionDistance * .75f;
                            _navigationController.SetDestination(nearestSuitable.Building.GetNearestPoint(transform.position));
                        }
                        else
                        {
                            Debug.Log("Взял ресы");

                            if (_transferring.TotalCount() == 0)
                            {
                                nearestSuitable.TransferResources(_transferring);
                            }

                            _transferring += nearestSuitable.Resources;
                            _transferring = nearestSuitable.Resources.GetClampedResources(
                                _buildingTask.Target.Price - _buildingTask.Target.StartTransferring, _maxResources);
                            nearestSuitable.TransferResources(-_transferring);

                            _buildingTask.Target.StartTransferring += _transferring;
                        }
                    }
                    else
                    {
                        _workType = WorkType.TheStoragesDoNotHaveTheNecessaryResources;

                        Debug.Log("Нету подходящего склада");
                    }
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, _buildingTask.Target.GetNearestPoint(transform.position)) > _actionDistance)
                {
                    _navigationController.SetDestination(_buildingTask.Target.GetNearestPoint(transform.position));
                }
                else
                {
                    Debug.Log("Строю дом епта");

                    _buildingTask.Target.IncreaseBuildingProgress();
                }
            }
        }

        private void UpdateBuilderLogic()
        {
            if (_buildingTask != null)
            {
                UpdateBuildingTask();
            }
            else
            {
                UpdateDestroyingTask();
            }
        }

        private void UpdateMasonLogic()
        {
            if (_masonTask.Target.IsCollected)
            {
                CurrentTask = null;
                return;
            }

            if (_transferring.TotalCount() == _maxResources)
            {
                TransferResourcesOnStorage();
            }
            else
            {
                if (Vector3.Distance(transform.position, _masonTask.Target.transform.position) > _actionDistance)
                {
                    _navigationController.SetDestination(_masonTask.Target.transform.position);
                }
                else
                {
                    _masonTask.Target.DecreaseStrength(this);
                }
            }
        }

        private void UpdateLoggerLogic()
        {
            if (_loggerTask.Target.IsCollected)
            {
                CurrentTask = null;
                _workType = WorkType.None;
                return;
            }

            if (_transferring.TotalCount() == _maxResources)
            {
                TransferResourcesOnStorage();
            }
            else
            {
                if (Vector3.Distance(transform.position, _loggerTask.Target.transform.position) > _actionDistance)
                {
                    _navigationController.SetDestination(_loggerTask.Target.transform.position);
                }
                else
                {
                    _loggerTask.Target.DecreaseStrength(this);
                }
            }
        }

        private void UpdateMoverLogic()
        {
            if (_transferring.TotalCount() >= _maxResources)
            {
                TransferResourcesOnStorage();
            }
            else
            {
                if (Vector3.Distance(transform.position, _moveTask.Target.GetNearestPoint(transform.position)) > _actionDistance)
                {
                    _navigationController.SetDestination(_moveTask.Target.GetNearestPoint(transform.position));
                }
                else
                {
                    var difference = _moveTask.Resources.
                        GetClampedResources(_moveTask.Resources, _maxResources - _transferring.TotalCount());

                    _moveTask.Resources -= difference;
                    _transferring += difference;
                }
            }
        }

        private void TransferResourcesOnStorage()
        {
            Debug.Log("Несу добытые ресы на склад");

            StorageHouse nearest = null;

            foreach (var storage in World.Storages)
            {
                if (nearest == null || Vector3.Distance(transform.position, nearest.Building.GetNearestPoint(transform.position)) >
                                Vector3.Distance(transform.position, storage.Building.GetNearestPoint(transform.position)))
                {
                    nearest = storage;
                }
            }

            if (Vector3.Distance(transform.position, nearest.Building.GetNearestPoint(transform.position)) > _actionDistance)
            {
                _navigationController.SetDestination(nearest.Building.GetNearestPoint(transform.position));
            }
            else
            {
                nearest.TransferResources(_transferring);
                _transferring = Resources.Empty;
            }
        }

        private void UpdateProfession()
        {
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

        private void UpdateParameters()
        {
            _navigationController.speed = _movingSpeed * MovingEfficiency;

            _navigationController.stoppingDistance = _actionDistance * .75f;

            _hunger += Time.deltaTime / Random.Range(20, 40);
        }

        private bool TryToEat()
        {
            var suitableList = World.Storages.FindAll(x => x.Resources.Contains(new Resources(1, 0, 0)));

            if (suitableList.Count > 0)
            {
                StorageHouse nearestSuitable = null;

                foreach (var suitable in suitableList)
                {
                    if (nearestSuitable == null || Vector3.Distance(transform.position,
                        nearestSuitable.Building.GetNearestPoint(transform.position)) >
                        Vector3.Distance(transform.position, suitable.Building.GetNearestPoint(transform.position)))
                    {
                        nearestSuitable = suitable;
                    }
                }

                if (Vector3.Distance(transform.position, nearestSuitable.Building.GetNearestPoint(transform.position)) > _actionDistance)
                {
                    _navigationController.SetDestination(nearestSuitable.Building.GetNearestPoint(transform.position));
                }
                else
                {
                    nearestSuitable.TransferResources(_transferring);
                    nearestSuitable.TransferResources(-new Resources(1, 0, 0));

                    _hunger = 0;
                }

                return true;
            }

            return false;
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            World.Villagers.Add(this);
        }

        private void OnDisable()
        {
            if (gameObject.scene.isLoaded)
            {
                World.Villagers.Remove(this);
            }
        }

        private void Start()
        {
            _navigationController = GetComponent<NavMeshAgent>();

            _movingSpeed = _navigationController.speed;
        }

        private void FixedUpdate()
        {
            _navigationController.isStopped = false;

            UpdateParameters();

            if (_hunger >= 1)
            {
                Die();
                return;
            }
            else if (_hunger > .75f && TryToEat())
            {
                return;
            }

            if (CurrentTask == null)
            {
                if (_transferring.TotalCount()  > 0)
                {
                    TransferResourcesOnStorage();
                }
                else if(_hunger > .5f)
                {
                    TryToEat();
                }
                else
                {
                    _navigationController.isStopped = true;
                }
                
                return;
            }

            if (_moveTask != null)
            {
                UpdateMoverLogic();

                return;
            }

            UpdateProfession();
        }
    }
}