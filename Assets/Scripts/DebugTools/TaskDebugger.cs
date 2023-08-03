using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VillagerSystem;

namespace Assets.Scripts.DebugTools
{
    internal class TaskDebugger : MonoBehaviour
    {
        [SerializeField] private List<MoveResourcesTask> _moveResourcesTasks = new List<MoveResourcesTask>();

        private void Update()
        {
            _moveResourcesTasks = TasksForVillager.MoveResourcesTasks;
        }
    }
}