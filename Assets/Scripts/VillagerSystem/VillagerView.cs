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


            switch (_villager.CurrentWork)
            {
                case Villager.WorkType.None:
                    _selectableUI.SetDescription("No job right now.");
                    break;
                case Villager.WorkType.IGoToTheStorage:
                    _selectableUI.SetDescription("I go to the storages to get resources.");
                    break;
                case Villager.WorkType.IBringResources:
                    _selectableUI.SetDescription("I carry resources for the building.");
                    break;
                case Villager.WorkType.TheStoragesDoNotHaveTheNecessaryResources:
                    _selectableUI.SetDescription("There are no resources to build in warehouses.");
                    break;
            }

            if (_villager.Profession == Villager.ProfessionType.None)
            {
                _selectableUI.SetHeader("Unemployed");
                //_selectableUI.SetDescription("Build a work building to get him a profession");
            }
            else if (_villager.Profession == Villager.ProfessionType.Builder)
            {
                _renderer.material = _builder;
                //_selectableUI.SetDescription("The builder follows your orders to build buildings");
            }
            else if(_villager.Profession == Villager.ProfessionType.Logger)
            {
                _renderer.material = _logger;
                _selectableUI.SetDescription("The logger follows your orders to collection wood");
            }
            else if(_villager.Profession == Villager.ProfessionType.Mason)
            {
                _renderer.material = _mason;
                //_selectableUI.SetDescription("Mason carries out orders for the collection of stone");
            }
        }
    }
}