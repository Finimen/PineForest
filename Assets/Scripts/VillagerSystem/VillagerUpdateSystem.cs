using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class VillagerUpdateSystem : MonoBehaviour
    {
        private void FixedUpdate()
        {
            UpdateMoveLogic();
            UpdateBuilders();
            UpdateLoggers();
            UpdateMasons();
        }

        private void UpdateBuilders()
        {
            if(TasksForVillager.CreateBuildingTasks.Count != 0)
            {
                var builders = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Builder);

                foreach (var builder in builders)
                {
                    if (builder.CurrentTask == null)
                    {
                        builder.SetTask(TasksForVillager.CreateBuildingTasks[0]);
                    }
                    else if (builder.CurrentWork == Villager.WorkType.WaitingForOtherBuildersToBringResources)
                    {
                        var otherTask = TasksForVillager.CreateBuildingTasks.Find(x => x != builder.CurrentTask);

                        if(otherTask != null)
                        {
                            builder.SetTask(otherTask);
                        }
                    }
                }
            }

            if (TasksForVillager.DestroyBuildingTasks.Count != 0)
            {
                var builders = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Builder);

                foreach (var builder in builders)
                {
                    if (builder.CurrentTask == null)
                    {
                        builder.SetTask(TasksForVillager.DestroyBuildingTasks[0]);
                    }
                }
            }
        }

        private void UpdateLoggers()
        {
            if (TasksForVillager.GetTreeTasks.Count == 0)
            {
                return;
            }

            var loggers = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Logger);

            foreach (var logger in loggers)
            {
                if (logger.CurrentTask == null)
                {
                    logger.SetTask(TasksForVillager.GetTreeTasks[0]);
                }
            }
        }

        private void UpdateMasons()
        {
            if (TasksForVillager.GetRockTasks.Count == 0)
            {
                return;
            }

            var masons = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Mason);

            foreach (var mason in masons)
            {
                if (mason.CurrentTask == null)
                {
                    mason.SetTask(TasksForVillager.GetRockTasks[0]);
                }
            }
        }

        private void UpdateMoveLogic()
        {
            if (TasksForVillager.MoveResourcesTasks.Count == 0)
            {
                return;
            }

            var villagers = World.Villagers.FindAll(x => x.CurrentTask == null);

            if(villagers.Count == 0)
            {
                return;
            }

            var iterationCount = Mathf.Min(villagers.Count, TasksForVillager.MoveResourcesTasks[0].Resources.TotalCount() / villagers[0].MaxResources);
            
            for (int i = 0; i < iterationCount; i++)
            {
                villagers[i].SetTask(TasksForVillager.MoveResourcesTasks[0]);
            }
        }
    }
}