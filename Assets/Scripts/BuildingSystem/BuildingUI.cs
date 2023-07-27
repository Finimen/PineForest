using Assets.Scripts.GameLogSystem;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(Building), typeof(SelectableObjectUI))]
    public class BuildingUI : MonoBehaviour
    {
        private string _descriptionWhenObjectIsBuilt;

        private Building _building;
        private SelectableObjectUI _selectableUI;

        private void SetBuiltDescription()
        {
            FindObjectOfType<GameLogger>()?.SendLog($"{_selectableUI.Header} is built", GameLogSystem.LogType.Info);

            _selectableUI.SetDescription(_descriptionWhenObjectIsBuilt);
            enabled = false;
        }

        private void Start()
        {
            _building = GetComponent<Building>();
            _selectableUI = GetComponent<SelectableObjectUI>();

            _descriptionWhenObjectIsBuilt = _selectableUI.Description;

            if (_building.IsPlaced)
            {
                SetBuiltDescription();
            }
            else
            {
                _building.OnPlaced += SetBuiltDescription;
            }
        }

        private void Update()
        {
            _selectableUI.SetDescription(
                $"Resources brought: {(int)(_building.Transferred.TotalCount() * 100f / _building.Price.TotalCount())}%\n" +
                $"Building progress: {(int)(_building.BuildingProgress * 100f)}%");
        }
    }
}