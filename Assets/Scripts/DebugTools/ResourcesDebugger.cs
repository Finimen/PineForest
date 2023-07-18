using UnityEngine;

namespace Assets.Scripts.DebugTools
{
    [ExecuteAlways]
    public class ResourcesDebugger : MonoBehaviour
    {
        [SerializeField] private int _villagerMaxResources;
        
        [Space(25)]
        [SerializeField] private Resources _villagerStored;
        [SerializeField] private Resources _villagerNeeded;
        
        [Space(25)]
        [SerializeField] private Resources _storageStored;

        [SerializeField] private bool _trans;

        private void Update()
        {
            if(_trans)
            {
                _trans = false;

                _villagerStored += _storageStored.GetClampedResources(_villagerNeeded, _villagerMaxResources - _villagerStored.TotalCount());
            }
        }
    }
}