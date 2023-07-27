using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class ScreenResolution : MonoBehaviour
    {
        [SerializeField] private bool _isFullScreen = true;

        private void Start()
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            dropdown.ClearOptions();

            var resolutions = Screen.resolutions;

            int currentResolution = 0;

            TMP_Dropdown.OptionData[] options = new TMP_Dropdown.OptionData[resolutions.Length];

            for (int i = 0; i < resolutions.Length; i++)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData($"{resolutions[i].width}x{resolutions[i].height} {(int)resolutions[i].refreshRateRatio.value}Hz");
                options[i] = option;
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolution = i;
                }
            }

            dropdown.AddOptions(options.ToList());
            dropdown.value = currentResolution;

            dropdown.onValueChanged.AddListener(SetScreenResolution);
        }

        private void SetScreenResolution(int id)
        {
            Screen.SetResolution(Screen.resolutions[id].width, Screen.resolutions[id].height, _isFullScreen);
        }
    }
}