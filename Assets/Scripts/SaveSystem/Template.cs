using Assets.Scripts.BuildingSystem;
using Assets.Scripts.VillagerSystem;
using System;

namespace Assets.Scripts.SaveSystem
{
    public class BaseTemplate<T>
    {
        public string Name;
        public T Template;
    }

    [Serializable]
    public class BuildingTemplate : BaseTemplate<Building>
    {

    }

    [Serializable]
    public class MinedResourceTemplate : BaseTemplate<MinedResource>
    {

    }

    [Serializable]
    public class VillagerTemplate : BaseTemplate<Villager>
    {

    }
}