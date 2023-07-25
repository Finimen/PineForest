using Assets.Scripts.BuildingSystem;
using Assets.Scripts.Environment;
using Assets.Scripts.VillagerSystem;
using System.Collections.Generic;

namespace Assets.Scripts
{
    internal static class World
    {
        public static List<Villager> Villagers { get; private set; } = new List<Villager>();
        public static List<StorageHouse> Storages { get; private set; } = new List<StorageHouse>();
        public static List<LightSource> LightSources { get; private set; } = new List<LightSource>();

        public static float WorkEfficiency { get; set; } = 1;
        public static float SunEfficiency { get; set; } = 1;
        public static float RainEfficiency { get; set; } = 0;
    }
}