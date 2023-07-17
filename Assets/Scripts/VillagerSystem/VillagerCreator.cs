using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class VillagerCreator : MonoBehaviour
    {
        [SerializeField] private Villager _villager;

        [SerializeField] private Transform _parent;

        public void CreateVillagers(Vector3 position, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                var villager = Instantiate(_villager, position.Randomize(5), Quaternion.identity, _parent);
            }
        }
    }
}