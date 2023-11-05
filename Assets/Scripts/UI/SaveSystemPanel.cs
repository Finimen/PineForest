using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SaveSystemPanel : MonoBehaviour
    {
        [SerializeField] private SaveTemplate _uiTemplate;
        
        [SerializeField] private Transform _savesParent;
        [SerializeField] private InputField _saveName;

        private SaveManager _saveManager;

        private void Start()
        {
            _saveManager = FindObjectOfType<SaveManager>();
        }

        public void SaveCurrent()
        {
            throw new System.Exception();
        }

        public void Render()
        {
            var saves = _saveManager.GetLoadFiles();

            foreach (var save in saves)
            {
                var saveUI = Instantiate(_uiTemplate);
                
                saveUI.Initialize(save, () =>
                {
                    _saveManager.SetSaveName(save);
                    _saveManager.SaveCurrent();
                },
                () =>
                {
                    _saveManager.SetSaveName(save);
                    _saveManager.LoadCurrent();
                },
                () =>
                {
                    _saveManager.SetSaveName(save);
                    _saveManager.DeleteCurrent();
                });
            }
        }
    }
}