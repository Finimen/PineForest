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

        public void UpdateMaterial(BuildAbleType buildAbleType)
        {
            if(_default == null)
            {
                _default = GetComponent<MeshRenderer>().material;
            }

            switch (buildAbleType)
            {
                case BuildAbleType.Unable:
                    GetComponent<MeshRenderer>().material = _unableToBuild;
                    break;
                case BuildAbleType.Able:
                    GetComponent<MeshRenderer>().material = _ableToBuild;
                    break;
                case BuildAbleType.Plan:
                    GetComponent<MeshRenderer>().material = _plan;
                    break;
                case BuildAbleType.Placed:
                    GetComponent<MeshRenderer>().material = _default;
                    break;
            }
        }
    }
}