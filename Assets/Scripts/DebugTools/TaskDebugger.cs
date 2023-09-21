using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VillagerSystem;

namespace Assets.Scripts.DebugTools
{
    internal class TaskDebugger : MonoBehaviour
    {
        [SerializeField] private List<MoveResourcesTask> _moveResourcesTasks;
        
        [SerializeField] private List<BuildingTask> _buildingTasks;
        [SerializeField] private List<DestroyBuildingTask> _destroyBuildingTasks;

        [SerializeField] private List<GetTreeTask> _getTreeTasks;
        [SerializeField] private List<GetRockTask> _getRockTask;

        private void Update()
        {
            _moveResourcesTasks = TasksForVillager.MoveResourcesTasks;

            _buildingTasks = TasksForVillager.CreateBuildingTasks;
            _destroyBuildingTasks = TasksForVillager.DestroyBuildingTasks;

            _getTreeTasks = TasksForVillager.GetTreeTasks;
            _getRockTask = TasksForVillager.GetRockTasks;
        }
    }
}