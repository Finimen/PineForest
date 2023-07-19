using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    [RequireComponent(typeof(Villager), typeof(SelectableObjectUI))]
    public class VillagerView : MonoBehaviour
    {
        [SerializeField] private Material _builder;
        [SerializeField] private Material _logger;
        [SerializeField] private Material _mason;

        private SelectableObjectUI _selectableUI;

        private Villager _villager;
        private MeshRenderer _renderer;

        private void Start()
        {
            _villager = GetComponent<Villager>();
            _renderer = GetComponent<MeshRenderer>();
            _selectableUI = GetComponent<SelectableObjectUI>();
        }

        private void Update()
        {
            _selectableUI.SetHeader(_villager.Profession.ToString());

            if (_villager.Profession == Villager.ProfessionType.None)
            {
                _selectableUI.SetHeader("Unemployed");
                _selectableUI.SetDescription("Build a work building to get him a profession");
            }
            else if (_villager.Profession == Villager.ProfessionType.Builder)
            {
                _renderer.material = _builder;
                _selectableUI.SetDescription("The builder follows your orders to build buildings");
            }
            else if(_villager.Profession == Villager.ProfessionType.Logger)
            {
                _renderer.material = _logger;
                _selectableUI.SetDescription("The logger follows your orders to collection wood");
            }
            else if(_villager.Profession == Villager.ProfessionType.Mason)
            {
                _renderer.material = _mason;
                _selectableUI.SetDescription("Mason carries out orders for the collection of stone");
            }
        }
    }
}