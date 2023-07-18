using UnityEngine;

namespace Assets.Scripts.BuildingSystem
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BuildingPart : MonoBehaviour
    {
        public enum BuildAbleType
        {
            Unable,
            Able,
            Plan,
            Placed
        }

        [SerializeField] private Material _ableToBuild;
        [SerializeField] private Material _unableToBuild;
        [SerializeField] private Material _plan;

        private Material _default;

        private MeshRenderer _renderer;

        public void UpdateMaterial(BuildAbleType buildAbleType)
        {
            switch (buildAbleType)
            {
                case BuildAbleType.Unable:
                    _renderer.material = _unableToBuild;
                    break;
                case BuildAbleType.Able:
                    _renderer.material = _ableToBuild;
                    break;
                case BuildAbleType.Plan:
                    _renderer.material = _plan;
                    break;
                case BuildAbleType.Placed:
                    _renderer.material = _default;
                    break;
            }
        }

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            _default = _renderer.material;
        }
    }
}