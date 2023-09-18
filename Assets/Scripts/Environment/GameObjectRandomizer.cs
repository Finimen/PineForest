using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class GameObjectRandomizer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _templates;

        private void Awake ()
        {
            Instantiate(_templates[Random.Range(0, _templates.Length)], transform.position, transform.rotation, transform);
        }
    }
}