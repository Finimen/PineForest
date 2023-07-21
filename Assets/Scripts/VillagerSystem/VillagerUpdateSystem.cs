using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    internal class VillagerUpdateSystem : MonoBehaviour
    {
        private void FixedUpdate()
        {
            UpdateBuilders();
            UpdateLoggers();
            UpdateMasons();
        }

        private void UpdateBuilders()
        {
            if(TasksForVillager.BuildingTasks.Count == 0)
            {
                return;
            }

            var builders = World.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Builder);

            foreach (var builder in builders)
            {
                if(builder.CurrentTask == null)
                {
                    Debug.Log("BuilderTaskSetted");
                    builder.SetTask(TasksForVillager.BuildingTasks.Peek());
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