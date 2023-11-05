using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SaveSystem
{
    public class SaveTemplate : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _date;

        [SerializeField] private Button _save;
        [SerializeField] private Button _load;
        [SerializeField] private Button _delete;

        public void Initialize(string name, Action onSaveClick, Action onLoadClick, Action onDeleteAction)
        {
            _name.text = name;

            _save.onClick.AddListener(onSaveClick.Invoke);
            _load.onClick.AddListener(onLoadClick.Invoke);
            _delete.onClick.AddListener(onDeleteAction.Invoke);
        }
    }
}