using Assets.Scripts.SaveSystem.Data;
using Assets.Scripts.BuildingSystem;
using Assets.Scripts.VillagerSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Assets.Scripts.WeatherSystem;

namespace Assets.Scripts.SaveSystem
{
    internal class SaveManager : MonoBehaviour
    {
        [SerializeField] private string _saveName = "main";
        [SerializeField] private string _folder = "/saves/";

        [Space(25), Header("Buildings")]
        [SerializeField] private Transform _buildingsParent;
        [SerializeField] private Building[] _buildingTemplates;

        [Space(25), Header("Resources")]
        [SerializeField] private Transform _resourcesParent;
        [SerializeField] private MinedResource[] _resourcesTemplates;

        [Space(25), Header("Villagers")]
        [SerializeField] private Transform _villagersParent;
        [SerializeField] private Villager _villagerTemplate;

        public void SetSaveName(string saveName)
        {
            _saveName = saveName;
        }

        public void SetFolder(string folder)
        {
            _folder = folder;
        }

        public void ClearFolder()
        {
            foreach (var file in GetLoadFiles())
            {
                File.Delete(file.Replace(@"\", @"/"));
            }
        }

        public void DeleteCurrent()
        {
            foreach (var file in GetLoadFiles())
            {
                if(file.Replace(@"\", @"/") == _saveName.Replace(@"\", @"/"))
                {
                    File.Delete(_saveName.Replace(@"\", @"/"));
                }
            }
        }

        [ContextMenu(nameof(GetLoadFiles))]
        public string[] GetLoadFiles()
        {
            string savePath = Application.persistentDataPath + _folder;

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string[] saveFiles = Directory.GetFiles(savePath);

            return saveFiles;
        }

        [ContextMenu("Save")]
        public void SaveCurrent()
        {
            var buildings = FindObjectsOfType<Building>();
            var villagers = FindObjectsOfType<Villager>();
            var minedResources = FindObjectsOfType<MinedResource>();

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
                    Id = building.Id,

                    StoredResources = building.GetComponent<StorageHouse>() != null ? 
                    building.GetComponent<StorageHouse>().Resources : Resources.Empty, 
                };

                buildingData.Add(data);
            }

            foreach(var minedResource in minedResources)
            {
                var data = new MinedResourceData()
                {
                    Position = minedResource.transform.position,
                    Rotation = minedResource.transform.rotation,

                    Id = minedResource.Id,
                    IsIsCollected = minedResource.IsCollected,
                };

                minedResourceData.Add(data);
            }

            foreach (var villager in villagers)
            {
                var data = new VillagerData()
                {
                    Position = villager.transform.position,
                    Rotation = villager.transform.rotation,
                    ProfessionType = villager.Profession,
                };

                villagerData.Add(data);
            }

            var time = FindObjectOfType<ChangerDayAndNight>().CurrentTime;
            var weather = FindObjectOfType<WeatherSystem.WeatherSystem>().CurrentIndex;

            SceneData sceneData = new SceneData(buildingData, minedResourceData, villagerData, time, weather);

            SerializationManager.Save(_saveName, _folder, sceneData);
        }

        [ContextMenu("Load")]
        public void LoadCurrent()
        {
            DestroySpawnedObjects();

            var buildings = new List<Building>();
            var resources = new List<MinedResource>();
            var villagers = new List<Villager>();

            UnityEngine.Debug.Log(Application.persistentDataPath + _folder + _saveName + ".save");

            SceneData sceneData = (SceneData)SerializationManager.Load(Application.persistentDataPath + _folder + _saveName + ".save");

            foreach (var buildingData in sceneData.Buildings)
            {
                try
                {
                    var buildingClone = Instantiate(_buildingTemplates.Where(x => x.Id == buildingData.Id).ToArray()[0],
                    buildingData.Position, buildingData.Rotation, _buildingsParent);

                    buildingClone.name = buildingClone.name.Replace(" (Clone) ", "");

                    if (buildingData.IsPlaced)
                    {
                        buildingClone.Place();
                    }

                    buildings.Add(buildingClone);

                    if (buildingData.StoredResources != Resources.Empty)
                    {
                        buildingClone.GetComponent<StorageHouse>().SetResources(buildingData.StoredResources);
                    }
                }
                catch
                {
                    Debug.LogError(buildingData.Id);
                }
            }

            foreach (var resourceData in sceneData.MinedResources)
            {
                if (!resourceData.IsIsCollected)
                {
                    var resourceClone = Instantiate(_resourcesTemplates.Where(x => x.Id == resourceData.Id).ToArray()[0],
                    resourceData.Position, resourceData.Rotation, _resourcesParent);

                    resourceClone.name = resourceClone.name.Replace(" (Clone) ", "");

                    resources.Add(resourceClone);
                }
            }

            foreach (var villagerData in sceneData.Villagers)
            {
                var villagerClone = Instantiate(_villagerTemplate, villagerData.Position, villagerData.Rotation, _villagersParent);

                villagerClone.ChangeProfession(villagerData.ProfessionType);

                villagers.Add(villagerClone);
            }

            FindObjectOfType<ChangerDayAndNight>().SetTime(sceneData.Time);
            FindObjectOfType<WeatherSystem.WeatherSystem>().SetWeather(sceneData.Weather);
        }

        public bool HashSaveFile()
        {
            return SerializationManager.HashSaveFile(Application.persistentDataPath + _folder + _saveName + ".save");
        }

        private void DestroySpawnedObjects()
        {
            foreach (var building in FindObjectsOfType<Building>())
            {
                Destroy(building.gameObject); 
            }

            foreach (var villager in FindObjectsOfType<Villager>())
            {
                Destroy(villager.gameObject);
            }

            foreach(var resource in FindObjectsOfType<MinedResource>())
            {
                Destroy(resource.gameObject);
            }
        }
    }
} 