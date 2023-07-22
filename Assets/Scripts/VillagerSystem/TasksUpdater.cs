using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class TasksUpdater : MonoBehaviour
    {
        private void Update()
        {
            if (TasksForVillager.CreateBuildingTasks.Count > 0)
            {
                for (int i = 0; i < TasksForVillager.CreateBuildingTasks.Count; i++)
                {
                    if(TasksForVillager.CreateBuildingTasks[i].Target == null || TasksForVillager.CreateBuildingTasks[i].Target.IsPlaced)
                    {
                        TasksForVillager.CreateBuildingTasks.RemoveAt(i);
                    }
                }
            }

            if (TasksForVillager.DestroyBuildingTasks.Count > 0 && TasksForVillager.DestroyBuildingTasks[0].Target == null)
            {
                for (int i = 0; i < TasksForVillager.DestroyBuildingTasks.Count; i++)
                {
                    if (TasksForVillager.DestroyBuildingTasks[i].Target == null)
                    {
                        TasksForVillager.DestroyBuildingTasks.RemoveAt(i);
                    }
                }
            }

            if (TasksForVillager.GetTreeTasks.Count > 0)
            {
                for (int i = 0; i < TasksForVillager.GetTreeTasks.Count; i++)
                {
                    if (TasksForVillager.GetTreeTasks[i].Target.IsCollected)
                    {
                        TasksForVillager.GetTreeTasks.RemoveAt(i);
                    }
                }
            }

            if (TasksForVillager.GetRockTasks.Count > 0 && TasksForVillager.GetRockTasks[0].Target.IsCollected)
            {
                for (int i = 0; i < TasksForVillager.GetRockTasks.Count; i++)
                {
                    if (TasksForVillager.GetRockTasks[i].Target.IsCollected)
                    {
                        TasksForVillager.GetRockTasks.RemoveAt(i);
                    }
                }
            }
        }
    }
}