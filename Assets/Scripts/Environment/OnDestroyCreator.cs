using UnityEngine;

namespace Assets.Scripts
{
    public class OnDestroyCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _template;

        private void OnDestroy()
        {
            if(gameObject.scene.isLoaded)
            {
                Instantiate(_template, transform.position, transform.rotation);
            }
        }
    }
}