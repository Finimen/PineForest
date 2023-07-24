using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class QualityLevelChanger : MonoBehaviour
    {
        private void Awake()
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            dropdown.ClearOptions();

            string[] names = QualitySettings.names;

            dropdown.AddOptions(names.ToList());

            dropdown.onValueChanged.AddListener(UpdateQualityLevel);
            dropdown.value = (int)QualitySettings.currentLevel;

            dropdown.onValueChanged.AddListener(UpdateQualityLevel);
        }

        private void UpdateQualityLevel(int level)
        {
            QualitySettings.SetQualityLevel(level, true);
        }
    }
}