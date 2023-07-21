using Assets.Scripts.BuildingSystem;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(Building))]
    public class LightSource : MonoBehaviour
    {
        [field: SerializeField] public float Intensity { get; private set; } = 1;

        private void Start ()
        {
            if(GetComponent<Building>().IsPlaced)
            {
                World.LightSources.Add(this);
            }
            else
            {
                GetComponent<Building>().OnPlaced += () => World.LightSources.Add(this);
            }
        }

        private void OnDisable()
        {
            World.LightSources.Remove(this);
        }
    }
}