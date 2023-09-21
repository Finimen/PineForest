using Assets.Scripts.SaveSystem.Data;
using Assets.Scripts.BuildingSystem;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Assets.Scripts.VillagerSystem;
using System.Linq;

namespace Assets.Scripts.SaveSystem
{
    internal class SaveManager : MonoBehaviour
    {
        [Serializable]
        public class BuildingTemplate
        {
            public string Name;
            public Building Template;
        }

        [SerializeField] private string saveName;
        [SerializeField] private string folder = "/saves/";

        [Space(25), Header("Buildings")]
        [SerializeField] private Transform _buildingsParent;
        [SerializeField] private BuildingTemplate[] _buildingTemplates;

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
        public void OnSave(List<Building> buildings, List<MinedResource> resources, List<Villager> villagers)
        {
            var buildingData = new List<BuildingData>();
            var minedResourceData = new List<MinedResourceData>();
            var villagerData = new List<VillagerData>();

            foreach (var building in buildings)
            {
                var data = new BuildingData()
                {
                    Transform = building.transform,
                    IsPlaced = building.IsPlaced,
                    Name = building.name,
                };

                buildingData.Add(data);
            }

            SceneData sceneData = new SceneData(buildingData, minedResourceData, villagerData);

            SerializationManager.Save(saveName, folder, sceneData);
        }

        [ContextMenu("Load")]
        public void OnLoad()
        {
            var buildings = new List<Building>();
            var resources = new List<MinedResource>();
            var villagers = new List<Villager>();

            UnityEngine.Debug.Log(Application.persistentDataPath + folder + saveName + ".save");

            SceneData sceneData = (SceneData)SerializationManager.Load(Application.persistentDataPath + folder + saveName + ".save");

            foreach (var buildingData in sceneData.Buildings)
            {
                var buildingClone = Instantiate(_buildingTemplates.Where(x => x.Name == buildingData.Name).ToArray()[0].Template, _buildingsParent);

                buildingClone.transform.position = buildingData.Transform.position;
                buildingClone.transform.rotation = buildingData.Transform.rotation;

                if (buildingData.IsPlaced)
                {
                    buildingClone.Place();
                }

                buildings.Add(buildingClone);
            }
        }
    }
}