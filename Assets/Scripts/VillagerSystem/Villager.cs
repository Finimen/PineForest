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

        [SerializeField] private float _buildDistance = 10f;
        [SerializeField] private float _mineDistance = 3.5f;

        [Space(25)]
        [SerializeField] private int _maxResources = 5;
        [SerializeField] private Resources _transferring;

        private NavMeshAgent _navigationController;

        private BuildingTask _buildingTask;
        private GetTreeTask _loggerTask;
        private GetRockTask _masonTask;

        [field: Space(25)]
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

        public void GiveResources(Resources resources)
        {
            Debug.Log("Я получил ресы");

            _transferring += resources.GetClampedResources(resources, _maxResources - _transferring.TotalCount());
        }

        private void UpdateBuilderLogic()
        {
            if (_buildingTask.Target.IsPlaced)
            {
                CurrentTask = null;
                return;
            }

            if (_buildingTask.Target.Transferred != _buildingTask.Target.Price)
            {
                if(_transferring.Contains(_buildingTask.Target.Needed))
                {
                    if (Vector3.Distance(transform.position, _buildingTask.Target.transform.position) > _buildDistance)
                    {
                        _navigationController.stoppingDistance = _buildDistance;
                        _navigationController.SetDestination(_buildingTask.Target.transform.position);
                        
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
                    Debug.Log("Ищю ресы на складах");

                    var suitableList = World.Storages.FindAll(x => x.Resources.Contains(_buildingTask.Target.Price));

                    if (suitableList.Count > 0)
                    {
                        StorageHouse nearestSuitable = null;

                        foreach (var suitable in suitableList)
                        {
                            if(nearestSuitable == null || Vector3.Distance(transform.position, nearestSuitable.transform.position) >
                                Vector3.Distance(transform.position, suitable.transform.position))
                            {
                                nearestSuitable = suitable;
                            }
                        }

                        if (Vector3.Distance(transform.position, nearestSuitable.transform.position) > _buildDistance)
                        {
                            Debug.Log("Иду к складу");
                            
                            _navigationController.stoppingDistance = _buildDistance;
                            _navigationController.SetDestination(nearestSuitable.transform.position);
                        }
                        else
                        {
                            Debug.Log("Взял ресы");

                            if(_transferring.TotalCount() == 0)
                            {
                                nearestSuitable.TransferResources(_transferring);
                            }

                            _transferring += nearestSuitable.Resources;
                            _transferring = nearestSuitable.Resources.GetClampedResources(
                                _buildingTask.Target.Price - _buildingTask.Target.Transferred,_maxResources);
                            nearestSuitable.TransferResources(-_transferring);
                        }
                    }
                    else
                    {
                        Debug.Log("Нету подходящего склада");
                    }
                }
            }
            else
            {
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
            {
                if (Vector3.Distance(transform.position, _masonTask.Target.transform.position) > _mineDistance)
                {
                    _navigationController.stoppingDistance = _mineDistance;
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
                return;
            }

            if(_transferring.TotalCount() == _maxResources)
            {
                TransferResourcesOnStorage();
            }
            else
            {
                if (Vector3.Distance(transform.position, _loggerTask.Target.transform.position) > _mineDistance)
                {
                    _navigationController.stoppingDistance = _mineDistance;
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
            Debug.Log("Сука инвентарь полный, нужно отнести добытые ресы на склад");

            StorageHouse nearest = null;

            foreach(var storage in World.Storages)
            {
                if(nearest == null || Vector3.Distance(nearest.transform.position, transform.position) >
                    Vector3.Distance(storage.transform.position, transform.position))
                {
                    nearest = storage;
                }
            }

            if (Vector3.Distance(transform.position, nearest.transform.position) > _buildDistance)
            {
                _navigationController.stoppingDistance = _mineDistance;
                _navigationController.SetDestination(_loggerTask.Target.transform.position);
            }
            else
            {
                nearest.TransferResources(_transferring);
                _transferring = Resources.Empty;
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