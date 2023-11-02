using UnityEngine;

namespace Assets.Scripts.SaveSystem
{
    public class SaveableObject : MonoBehaviour
    {
        [SerializeField] private int _id;

        public int Id => _id;
    }
}