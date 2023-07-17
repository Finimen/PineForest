﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    internal class VillagerUpdateSystem : MonoBehaviour
    {
        private List<Villager> _villagers = new List<Villager>();

        public List<Villager> Villagers => _villagers;

        private void FixedUpdate()
        {
            UpdateBuilders();
            UpdateLoggers();
        }

        private void UpdateBuilders()
        {
            if(TasksForVillager.BuildingTasks.Count == 0)
            {
                return;
            }

            var builders = Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Builder);

            foreach (var builder in builders)
            {
                if(builder.CurrentTask == null)
                {
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

            var loggers = Villagers.FindAll(x => x.Profession == Villager.ProfessionType.Logger);

            foreach (var logger in loggers)
            {
                if (logger.CurrentTask == null)
                {
                    logger.SetTask(TasksForVillager.GetTreeTasks.Peek());
                }
            }
        }
    }
}