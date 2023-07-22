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
        }

        [SerializeField] private float _actionDistance = 2.5f;

        [Space(25)]
        [SerializeField] private int _maxResources = 5;
        [SerializeField] private Resources _transferring;

        private NavMeshAgent _navigationController;

        private BuildingTask _buildingTask;
        private DestroyBuildingTask _destroyingTask;
        private GetTreeTask _loggerTask;
        private GetRockTask _masonTask;

        private WorkType _workType;

        private float _movingSpeed;

        [field: SerializeField, Space(25)] public ProfessionType Profession { get; private set; }
        public BaseVillagerTask CurrentTask { get; private set; }
        public float MovingEfficiency { get; set; } = 1;
        public WorkType CurrentWork => _workType;

        public void SetTask(BaseVillagerTask task)
        {
            CurrentTask = task;

            switch (Profession)
            {
                case ProfessionType.Builder:
                    if(CurrentTask is (BuildingTask))
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
                    _navigationController.stoppingDistance = _actionDistance * .75f;
                    _navigationController.SetDestination(_destroyingTask.Target.transform.position);
                }
                else
                {
                    _destroyingTask.Target.IncreaseDestroyingProgress();
                }
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

                            _navigationController.stoppingDistance = _actionDistance * .75f;
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
                    _navigationController.stoppingDistance = _actionDistance * .75f;
                    _navigationController.SetDestination(_buildingTask.Target.GetNearestPoint(transform.position));
                }
                else
                {
                    Debug.Log($"{gameObject.name} is building");

                    _buildingTask.Target.IncreaseBuildingProgress();
                }
            }
        }

        private void UpdateBuilderLogic()
        {
            if(_buildingTask != null)
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
                    _navigationController.stoppingDistance = _actionDistance * .75f;
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

            if(_transferring.TotalCount() == _maxResources)
            {
                TransferResourcesOnStorage();
            }
            else
            {
                if (Vector3.Distance(transform.position, _loggerTask.Target.transform.position) > _actionDistance)
                {
                    _navigationController.stoppingDistance = _actionDistance * .75f;
                    _navigationController.SetDestination(_loggerTask.Target.transform.position);
                }
                else
                {
                    _loggerTask.Target.DecreaseStrength(this);
                }
            }
        }

        private void TransferResourcesOnStorage()
        {
            Debug.Log("���� ������� ���� �� �����");

            StorageHouse nearest = null;

            foreach(var storage in World.Storages)
            {
                if(nearest == null || Vector3.Distance(nearest.transform.position, transform.position) >
                    Vector3.Distance(storage.transform.position, transform.position))
                {
                    nearest = storage;
                }
            }

            if (Vector3.Distance(transform.position, nearest.transform.position) > _actionDistance)
            {
                _navigationController.stoppingDistance = _actionDistance * .75f;
                _navigationController.SetDestination(nearest.transform.position);
            }
            else
            {
                nearest.TransferResources(_transferring);
                _transferring = Resources.Empty;
            }
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

        private void UpdateParameters()
        {
            _navigationController.speed = _movingSpeed * MovingEfficiency;
        }

        private void Start()
        {
            _navigationController = GetComponent<NavMeshAgent>();

            _movingSpeed = _navigationController.speed;
        }

        private void FixedUpdate()
        {
            UpdateParameters();

            if (CurrentTask == null)
            {
                if(_transferring.TotalCount()  > 0)
                {
                    TransferResourcesOnStorage();
                }
                else
                {
                    _navigationController.isStopped = true;
                }
                return;
            }

            _navigationController.isStopped = false;

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