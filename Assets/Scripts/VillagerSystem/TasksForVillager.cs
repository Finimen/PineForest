using Assets.Scripts.BuildingSystem;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.VillagerSystem
{
    public static class TasksForVillager
    {
        public static List<BuildingTask> CreateBuildingTasks { get; private set; } = new List<BuildingTask>();
        public static List<GetTreeTask> GetTreeTasks { get; private set; } = new List<GetTreeTask>();
        public static List<GetRockTask> GetRockTasks { get; private set; } = new List<GetRockTask>();
        public static List<DestroyBuildingTask> DestroyBuildingTasks { get; private set; } = new List<DestroyBuildingTask>();
        public static List<MoveResourcesTask> MoveResourcesTasks { get; private set; } = new List<MoveResourcesTask>();
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

    public class MoveResourcesTask : BaseVillagerTask
    {
        public Building Target;

        public Resources Resources;

        public MoveResourcesTask(Building target, Resources resources)
        {
            Target = target;

            Resources = resources;
        }

        public event Action OnGiven;

        public void Give()
        {
            OnGiven?.Invoke();
        }
    }

    public abstract class BaseVillagerTask
    {

    }
}