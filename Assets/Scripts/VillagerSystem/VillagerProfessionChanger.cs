using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class VillagerProfessionChanger : MonoBehaviour
    {
        private VillagerUpdateSystem _villagerSystem;

        public int BuildersToCreate { get; set; }
        public int LoggersToCreate { get; set; }

        private void Start()
        {
            _villagerSystem = FindObjectOfType<VillagerUpdateSystem>();
        }

        private void Update()
        {
            var unemployed = _villagerSystem.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.None);

            if(unemployed.Count == 0)
            {
                return;
            }

            if (BuildersToCreate > 0)
            {
                var loopCount = Mathf.Min(BuildersToCreate, unemployed.Count);

                for (int i = 0; i < loopCount; i++)
                {
                    BuildersToCreate--;
                    unemployed[i].ChangeProfession(Villager.ProfessionType.Builder);
                }
            }

            if (BuildersToCreate > 0)
            {
                var loopCount = Mathf.Min(LoggersToCreate, unemployed.Count);

                for (int i = 0; i < loopCount; i++)
                {
                    LoggersToCreate--;
                    unemployed[i].ChangeProfession(Villager.ProfessionType.Logger);
                }
            }
        }
    }
}