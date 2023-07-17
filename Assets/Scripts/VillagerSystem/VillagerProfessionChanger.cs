using UnityEngine;

namespace Assets.Scripts.VillagerSystem
{
    public class VillagerProfessionChanger : MonoBehaviour
    {
        private VillagerUpdateSystem _villagerSystem;

        public int BuilderToCreate { get; set; }

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

            if (BuilderToCreate > 0)
            {
                var loopCount = Mathf.Min(BuilderToCreate, unemployed.Count);

                for (int i = 0; i < loopCount; i++)
                {
                    BuilderToCreate--;
                    unemployed[i].ChangeProfession(Villager.ProfessionType.Builder);
                }
            }
        }
    }
}