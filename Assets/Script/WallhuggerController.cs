using System;
using UnityEngine;
using UnityEngine.AI;

namespace Script {
    [RequireComponent(typeof(NavMeshAgent))]
    public class WallhuggerController : MonoBehaviour {
        public enum State {
            Waiting,
            Roaming
        }
        
        public float maxRoamingDistance = 20.0f;
        
        private NavMeshAgent _agent;
        private State _state = State.Waiting;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            switch (_state) {
                case State.Waiting:
                    Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * maxRoamingDistance;
                    NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxRoamingDistance, 1);
                    Vector3 finalPosition = hit.position;
                    _agent.SetDestination(finalPosition);
                    _state = State.Roaming;
                    break;
                case State.Roaming:
                    bool reachedDestination = !_agent.pathPending && !_agent.isOnOffMeshLink &&
                                              (_agent.remainingDistance <= _agent.stoppingDistance ||
                                               _agent.velocity.magnitude < 0.15f);
                    if (reachedDestination) _state = State.Waiting;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
