using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.GameLogSystem
{
    public class GameLogger : MonoBehaviour
    {
        [SerializeField] private Transform _logsParent;

        [SerializeField] private ScaleController _education;
        [SerializeField] private DoScaler _selectableUI;

        [Space(25)]
        [SerializeField] private GameLog _template;

        [Space(15)]
        [SerializeField] private Color _warningColor;
        [SerializeField] private Color _actionColor;
        [SerializeField] private Color _infoColor;

        [Space(15)]
        [SerializeField] private float _warningLifeTime = 4.5f;
        [SerializeField] private float _actionLifeTime = 2.5f;
        [SerializeField] private float _infoLifeTime = 1.5f;

        public void SendLog(string message, LogType logType)
        {
            _education.SetActive(false);
            _selectableUI.SetScale(Vector3.zero);

            var log = Instantiate(_template, _logsParent);

            switch(logType)
            {
                case LogType.Warning:
                    log.UpdateUI(message, _infoColor, _warningLifeTime); 
                    break;
                case LogType.Action:
                    log.UpdateUI(message, _actionColor, _actionLifeTime);
                    break;
                case LogType.Info:
                    log.UpdateUI(message, _infoColor, _infoLifeTime);
                    break;
            }
        }
    }
}