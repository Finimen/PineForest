using UnityEngine;

namespace Assets.Scripts
{
    public static class Vector3Extensions
    {
        public static Vector3 Randomize(this Vector3 vector, float randomness)
        {
            return vector + new Vector3(Random.Range(-randomness, randomness), Random.Range(-randomness, randomness), Random.Range(-randomness, randomness));
        }
    }
}