using Assets.Scripts.SaveSystem.Data;
using Assets.Scripts.BuildingSystem;
using Assets.Scripts.VillagerSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Assets.Scripts.SaveSystem
{
    internal class SaveManager : MonoBehaviour
    {
        [SerializeField] private string saveName;
        [SerializeField] private string folder = "/saves/";

        [Space(25), Header("Buildings")]
        [SerializeField] private Transform _buildingsParent;
        [SerializeField] private Building[] _buildingTemplates;

        [Space(25), Header("Resources")]
        [SerializeField] private Transform _resourcesParent;
        [SerializeField] private MinedResource[] _resourcesTemplates;

        [Space(25), Header("Villagers")]
        [SerializeField] private Transform _villagersParent;
        [SerializeField] private Villager _villagerTemplates;

        public void SetSaveName(string saveName)
        {
            this.saveName = saveName;
        }

        public void SetFolder(string folder)
        {
            this.folder = folder;
        }

        public void ClearFolder()
        {
            foreach(var file in GetLoadFiles())
            {
                File.Delete(file.Replace(@"\", @"/"));
            }
        }

        [ContextMenu(nameof(GetLoadFiles))]
        public string[] GetLoadFiles()
        {
            string savePath = Application.persistentDataPath + folder;

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string[] saveFiles = Directory.GetFiles(savePath);

            return saveFiles;
        }

        [ContextMenu("Save")]
        public void Save()
        {
            var buildings = FindObjectsOfType<Building>();

            var buildingData = new List<BuildingData>();
            var minedResourceData = new List<MinedResourceData>();
            var villagerData = new List<VillagerData>();

            foreach (var building in buildings)
            {
                var data = new BuildingData()
                {
                    Position = building.transform.position,
                    Rotation = building.transform.rotation,
                    IsPlaced = building.IsPlaced,
                    Name = building.name,
                };

                buildingData.Add(data);
            }

            SceneData sceneData = new SceneData(buildingData, minedResourceData, villagerData);

            SerializationManager.Save(saveName, folder, sceneData);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            DestroySpawnedObjects();

            var buildings = new List<Building>();
            var resources = new List<MinedResource>();
            var villagers = new List<Villager>();

            UnityEngine.Debug.Log(Application.persistentDataPath + folder + saveName + ".save");

            SceneData sceneData = (SceneData)SerializationManager.Load(Application.persistentDataPath + folder + saveName + ".save");

            foreach (var buildingData in sceneData.Buildings)
            {
                Debug.Log(buildingData.Name);

                var buildingClone = Instantiate(_buildingTemplates.Where(x => x.name == buildingData.Name).ToArray()[0], _buildingsParent);

                buildingClone.transform.position = buildingData.Position;
                buildingClone.transform.rotation = buildingData.Rotation;

                if (buildingData.IsPlaced)
                {
                    buildingClone.Place();
                }

                buildings.Add(buildingClone);
            }
        }

        public bool HashSaveFile()
        {
            return SerializationManager.HashSaveFile(Application.persistentDataPath + folder + saveName + ".save");
        }

        private void DestroySpawnedObjects()
        {
            var buildings = FindObjectsOfType<Building>();

            foreach (var building in buildings)
            {
                Destroy(building.gameObject); 
            }
        }
    }
} 