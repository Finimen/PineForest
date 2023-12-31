using Assets.Scripts.BuildingSystem;
using Assets.Scripts.GameLogSystem;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.VillagerSystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Villager : SaveableObject
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

        [Space(25)]
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
        
        public bool IsMining { get; private set; }
        public bool IsGetting { get; private set; }
        public bool IsBuilding { get; private set; }

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
            Debug.Log("� ������� ����");

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
            IsBuilding = false;

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
                        _navigationController.SetDestination(_buildingTask.Target.GetNearestPoint(transform.position));

                        _workType = WorkType.IBringResources;
                        Debug.Log("���� ����");
                    }
                    else
                    {
                        Debug.Log("������� ����");

                        _buildingTask.Target.TransferResources(_transferring);
                        _transferring = Resources.Empty;
                    }
                }
                else
                {
                    if (_buildingTask.Target.Price == _buildingTask.Target.StartTransferring)
                    {
                        Debug.Log("��� ���� ������ ���� �������� ����");

                        _workType = WorkType.WaitingForOtherBuildersToBringResources;
                        return;
                    }

                    Debug.Log("��� ���� �� �������");

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
                            Debug.Log("��� � ������");

                            _workType = WorkType.IGoToTheStorage;

                            _navigationController.SetDestination(nearestSuitable.Building.GetNearestPoint(transform.position));
                        }
                        else
                        {
                            Debug.Log("���� ����");

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

                        Debug.Log("���� ����������� ������");
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
                    Debug.Log("����� ��� ����");

                    _buildingTask.Target.IncreaseBuildingProgress();

                    IsBuilding = true;
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
            IsMining = false;

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

                    IsMining = true;
                }
            }
        }

        private void UpdateLoggerLogic()
        {
            IsMining = false;

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
                    IsMining = true;

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
            Debug.Log("���� ������� ���� �� �����");

            StorageHouse nearest = null;

            foreach (var storage in World.Storages)
            {
                if(storage.Resources.TotalCount() + _transferring.TotalCount() <= storage.MaxResources)
                {
                    if (nearest == null || Vector3.Distance(transform.position, nearest.Building.GetNearestPoint(transform.position)) >
                                Vector3.Distance(transform.position, storage.Building.GetNearestPoint(transform.position)))
                    {
                        nearest = storage;
                    }
                }
            }

            if(nearest == null)
            {
                Debug.Log("��� ������ ���������!");

                return;
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

            _navigationController.stoppingDistance = _actionDistance * .6f;

            _hunger += Time.deltaTime / Random.Range(100, 150);
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

            FindObjectOfType<GameLogger>().SendLog("Villager died (No food)", GameLogSystem.LogType.Warning);
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
                else if(_hunger > .25f)
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