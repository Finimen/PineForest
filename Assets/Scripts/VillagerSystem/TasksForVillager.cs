using Assets.Scripts.BuildingSystem;
using System.Collections.Generic;

namespace Assets.Scripts.VillagerSystem
{
    public static class TasksForVillager
    {
        public static Queue<BuildingTask> CreateBuildingTasks { get; private set; } = new Queue<BuildingTask>();
        public static Queue<GetTreeTask> GetTreeTasks { get; private set; } = new Queue<GetTreeTask>();
        public static Queue<GetRockTask> GetRockTasks { get; private set; } = new Queue<GetRockTask>();
        public static Queue<DestroyBuildingTask> DestroyBuildingTasks { get; private set; } = new Queue<DestroyBuildingTask>();
    }

    public class BuildingTask : BaseVillagerTask
    {
        public Building Target;

        public BuildingTask(Building target)
        {
            Target = target;
        }
    }

    public class GetTreeTask : BaseVillagerTask
    {
        public MinedResource Target;

        public GetTreeTask(MinedResource target)
        {
            Target = target;
        }
    }

    public class GetRockTask : BaseVillagerTask
    {
        public MinedResource Target;

        public GetRockTask(MinedResource target)
        {
            Target = target;
        }
    }

    public class DestroyBuildingTask : BaseVillagerTask
    {
        public Building Target;

        public DestroyBuildingTask(Building target)
        {
            Target = target;
        }
    }

    public abstract class BaseVillagerTask
    {

    }
}