using Assets.Scripts.SaveSystem;
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
            if (Input.GetKeyDown(KeyCode.X))
            {
                _saver.SaveCurrent();
            }

            if (Input.GetKeyDown(KeyCode.C) && _saver.HashSaveFile())
            {
                _saver.LoadCurrent();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Reset");
            }
        }
    }
}