using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    [RequireComponent(typeof(Villager), typeof(SelectableObjectUI))]
    public class VillagerView : MonoBehaviour
    {
        private SelectableObjectUI _selectableUI;

        private Villager _villager;

        private void Start()
        {
            _villager = GetComponent<Villager>();
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
                    _selectableUI.SetDescription("Goes to storage for resources.");
                    break;
                case Villager.WorkType.IBringResources:
                    _selectableUI.SetDescription("Carries resources for construction.");
                    break;
                case Villager.WorkType.TheStoragesDoNotHaveTheNecessaryResources:
                    _selectableUI.SetDescription("There are no resources to build in warehouses.");
                    break;
            }
        }
    }
}