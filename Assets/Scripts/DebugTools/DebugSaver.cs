using Assets.Scripts.BuildingSystem;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.VillagerSystem;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.DebugTools
{
    public class DebugSaver : MonoBehaviour
    {
        private SaveManager _saver;

        private void Start()
        {
            _saver = FindObjectOfType<SaveManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                _saver.OnSave();
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _saver.OnLoad();
            }
        }
    }
}