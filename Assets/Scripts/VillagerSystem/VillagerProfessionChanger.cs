using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class VillagerProfessionChanger : MonoBehaviour
    {
        private VillagerUpdateSystem _villagerSystem;

        [field: SerializeField] public int BuildersToCreate { get; set; }
        [field: SerializeField] public int LoggersToCreate { get; set; }
        [field: SerializeField] public int MasonsToCreate { get; set; }

        private void Start()
        {
            _villagerSystem = FindObjectOfType<VillagerUpdateSystem>();
        }

        private void Update()
        {
            var unemployed = _villagerSystem.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.None);

            TryCreateBuilders(unemployed);

            unemployed = _villagerSystem.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.None);

            TryCreateLoggers(unemployed);
            
            unemployed = _villagerSystem.Villagers.FindAll(x => x.Profession == Villager.ProfessionType.None);

            TryCreateMasons(unemployed);
        }

        private void TryCreateBuilders(List<Villager> unemployed)
        {
            if (BuildersToCreate > 0)
            {
                var loopCount = Mathf.Min(BuildersToCreate, unemployed.Count);

                for (int i = 0; i < loopCount; i++)
                {
                    BuildersToCreate--;
                    unemployed[i].ChangeProfession(Villager.ProfessionType.Builder);
                }
            }
        }

        private void TryCreateLoggers(List<Villager> unemployed)
        {
            if (LoggersToCreate > 0)
            {
                var loopCount = Mathf.Min(LoggersToCreate, unemployed.Count);

                for (int i = 0; i < loopCount; i++)
                {
                    LoggersToCreate--;
                    unemployed[i].ChangeProfession(Villager.ProfessionType.Logger);
                }
            }
        }

        private void TryCreateMasons(List<Villager> unemployed)
        {
            if (MasonsToCreate > 0)
            {
                var loopCount = Mathf.Min(MasonsToCreate, unemployed.Count);

                for (int i = 0; i < loopCount; i++)
                {
                    MasonsToCreate--;
                    unemployed[i].ChangeProfession(Villager.ProfessionType.Mason);
                }
            }
        }
    }
}