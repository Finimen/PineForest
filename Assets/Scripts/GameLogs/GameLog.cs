using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameLogSystem
{
    [RequireComponent(typeof(DoScaler))]
    public class GameLog : MonoBehaviour 
    {
        [SerializeField] private TMP_Text _message;
        [SerializeField] private Image _color;

        [SerializeField] private float _lifeTime;

        public void UpdateUI(string message, Color color, float lifeTime)
        {
            _message.text = message;
            _color.color = color;

            _lifeTime = lifeTime;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_lifeTime);

            GetComponent<DoScaler>().SetDestroyOnPlayed(true);
            GetComponent<DoScaler>().SetScale(Vector3.zero);
        }
    }
}