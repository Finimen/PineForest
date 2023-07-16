using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(BuildingPlacer))]
    public class BuildingChanger : MonoBehaviour
    {
        [SerializeField] private Building[] _templates;

        private BuildingPlacer _placer;

        private void Start()
        {
            _placer = GetComponent<BuildingPlacer>();
        }

        private void Update()
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    _placer.PlaceBuilding(_templates[i]);
                }
            }
        }
    }
}