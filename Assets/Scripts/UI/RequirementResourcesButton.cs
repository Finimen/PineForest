using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class RequirementResourcesButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Resources _resources;

        [SerializeField] private int _unemployed;

        private RequirementResourcesCanvas _canvas;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _canvas.ShowUI();
            _canvas.UpdateUI(transform.position, _resources, _unemployed);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _canvas.HideUI();
        }

        private void Start()
        {
            _canvas = FindObjectOfType<RequirementResourcesCanvas>();
        }
    }
}