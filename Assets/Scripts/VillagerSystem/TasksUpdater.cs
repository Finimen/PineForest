using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class TasksUpdater : MonoBehaviour
    {
        private void Update()
        {
            if (TasksForVillager.BuildingTasks.Count > 0 && TasksForVillager.BuildingTasks.Peek().Target.IsPlaced)
            {
                TasksForVillager.BuildingTasks.Dequeue();
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