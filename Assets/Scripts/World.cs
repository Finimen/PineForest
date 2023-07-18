using Assets.Scripts.BuildingSystem;
using System.Collections.Generic;

namespace Assets.Scripts
{
    internal static class World
    {
        public static List<StorageHouse> Storages { get; private set; } = new List<StorageHouse>();
    }
}