using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class TasksUpdater : MonoBehaviour
    {
        [SerializeField] private int _createBuildingCount;

        private void Update()
        {
            _createBuildingCount = TasksForVillager.CreateBuildingTasks.Count;

            if (TasksForVillager.CreateBuildingTasks.Count > 0 && (TasksForVillager.CreateBuildingTasks.Peek().Target == null || TasksForVillager.CreateBuildingTasks.Peek().Target.IsPlaced))
            {
                TasksForVillager.CreateBuildingTasks.Dequeue();
            }

            if (TasksForVillager.DestroyBuildingTasks.Count > 0 && TasksForVillager.DestroyBuildingTasks.Peek().Target == null)
            {
                TasksForVillager.DestroyBuildingTasks.Dequeue();
            }

            if (TasksForVillager.GetTreeTasks.Count > 0 && TasksForVillager.GetTreeTasks.Peek().Target.IsCollected)
            {
                TasksForVillager.GetTreeTasks.Dequeue();
            }

            if (TasksForVillager.GetRockTasks.Count > 0 && TasksForVillager.GetRockTasks.Peek().Target.IsCollected)
            {
                TasksForVillager.GetRockTasks.Dequeue();
            }
        }
    }
}