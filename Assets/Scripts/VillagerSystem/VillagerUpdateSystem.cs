using System.Collections.Generic;
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
    }
}