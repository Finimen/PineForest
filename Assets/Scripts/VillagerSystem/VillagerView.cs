using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    [ExecuteAlways]
    [RequireComponent(typeof(Villager))]
    public class VillagerView : MonoBehaviour
    {
        [SerializeField] private Material _builder;
        [SerializeField] private Material _logger;
        [SerializeField] private Material _mason;

        private Villager _villager;
        private MeshRenderer _renderer;

        private void Start()
        {
            _villager = GetComponent<Villager>();
            _renderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if(_villager.Profession == Villager.ProfessionType.Builder)
            {
                _renderer.material = _builder;
            }
            else if(_villager.Profession == Villager.ProfessionType.Logger)
            {
                _renderer.material = _logger;
            }
            else if(_villager.Profession == Villager.ProfessionType.Mason)
            {
                _renderer.material = _mason;
            }
        }
    }
}