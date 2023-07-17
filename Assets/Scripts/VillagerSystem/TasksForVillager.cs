using Assets.Scripts.BuildingSystem;
using System.Collections.Generic;

namespace Assets.Scripts.VillagerSystem
{
    public static class TasksForVillager
    {
        public static Queue<BuildingTask> BuildingTasks { get; private set; } = new Queue<BuildingTask>();
    }

    public class BuildingTask : BaseVillagerTask
    {
        public Building Target;

        public BuildingTask(Building target)
        {
            Target = target;
        }
    }

    public abstract class BaseVillagerTask
    {

    }
}