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
        }
    }
}