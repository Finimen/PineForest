using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class RequirementResourcesCanvas : MonoBehaviour
    {
        [SerializeField] private DoScaler _panel;

        [SerializeField] private TMP_Text _wood;
        [SerializeField] private TMP_Text _stone;
        [SerializeField] private TMP_Text _food;
        [SerializeField] private TMP_Text _unemployed;

        [SerializeField] private float _moveDuration = .1f;

        [SerializeField] private Ease _moveEase = Ease.OutBounce;

        public void UpdateUI(Vector3 position, Resources resources, int unemployed = 0)
        {
            _panel.transform.DOMove(position, _moveDuration).SetEase(_moveEase);

            if(resources.Wood == 0)
            {
                _wood.gameObject.SetActive(false);
            }
            else
            {
                _wood.gameObject.SetActive(true);

                _wood.text = $"Wood: {resources.Wood}";
            }

            if (resources.Stone == 0)
            {
                _stone.gameObject.SetActive(false);
            }
            else
            {
                _stone.gameObject.SetActive(true);

                _stone.text = $"Stone: {resources.Stone}";
            }

            if (resources.Food == 0)
            {
                _food.gameObject.SetActive(false);
            }
            else
            {
                _food.gameObject.SetActive(true);

                _food.text = $"Food: {resources.Food}";
            }

            if (unemployed == 0)
            {
                _unemployed.gameObject.SetActive(false);
            }
            else
            {
                _unemployed.gameObject.SetActive(true);

                _unemployed.text = $"Unemployed: {unemployed}";
            }
        }

        public void ShowUI()
        {
            _panel.SetScale(Vector3.one);
        }

        public void HideUI()
        {
            _panel?.SetScale(Vector3.zero);
        }
    }
}