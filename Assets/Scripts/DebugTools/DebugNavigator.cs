using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.DebugTools
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class DebugNavigator : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _stoppingDistance;

        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _agent.stoppingDistance = _stoppingDistance;
            _agent.SetDestination(_target.position);
        }
    }
}