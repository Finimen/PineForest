using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class VillagerUpdateSystem : MonoBehaviour
    {
        private void FixedUpdate()
        {
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
                        Debug.Log("BuilderTaskSetted");
                        builder.SetTask(TasksForVillager.CreateBuildingTasks.Peek());
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
                        Debug.Log("BuilderTaskSetted");
                        builder.SetTask(TasksForVillager.DestroyBuildingTasks.Peek());
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
                    Debug.Log("LoggerTaskSetted");
                    logger.SetTask(TasksForVillager.GetTreeTasks.Peek());
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
                    Debug.Log("MasonTaskSetted");
                    mason.SetTask(TasksForVillager.GetRockTasks.Peek());
                }
            }
        }
    }
}