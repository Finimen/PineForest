using Assets.Scripts.FireSystems;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DebugTools
{
    [RequireComponent(typeof(FireComponent))]
    public class DebugFireEnabler : MonoBehaviour
    {
        [SerializeField] private float _delay = 5;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_delay);

            GetComponent<FireComponent>().IsFire = true;
        }
    }
}