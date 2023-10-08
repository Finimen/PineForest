using Assets.Scripts.WeatherSystem;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.SaveSystem.Data
{
    [Serializable] 
    internal struct SceneData
    {
        public List<BuildingData> Buildings;
        public List<MinedResourceData> MinedResources;
        public List<VillagerData> Villagers;

        public float Time;
        public int Weather;

        public SceneData(List<BuildingData> buildings, List<MinedResourceData> minedResources, List<VillagerData> villagers, float time, int weather)
        {
            Buildings = buildings;
            MinedResources = minedResources;
            Villagers = villagers;
            
            Time = time;
            Weather = weather;
        }
    }
}