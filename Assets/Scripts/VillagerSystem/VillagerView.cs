using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    [RequireComponent(typeof(Villager))]
    public class VillagerView : MonoBehaviour
    {
        [SerializeField] private Material _builder;
        [SerializeField] private Material _farmer;

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
            if(_villager.Profession == Villager.ProfessionType.Logger)
            {
                _renderer.material = _farmer;
            }
        }
    }
}