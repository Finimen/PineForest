using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class Villager : MonoBehaviour
    {
        public enum ProfessionType
        {
            None,
            Builder,
            Farmer
        }

        [field: SerializeField] public ProfessionType Profession { get; private set; }
        public BaseVillagerTask CurrentTask { get; private set; }

        public void SetTask(BaseVillagerTask task)
        {
            CurrentTask = task;
        }
    }
}