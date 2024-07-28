using System;
using UnityEngine;
using UnityEngine.AI;

namespace Script {
    [RequireComponent(typeof(NavMeshAgent))]
    public class WallhuggerController : MonoBehaviour {
        public enum State {
            Waiting,
            Roaming,
            Hunting
        }

        public int id = 1;
        public float maxRoamingDistance = 20.0f;
        public State state = State.Waiting;
        
        private NavMeshAgent _agent;
        private Transform _playerTransform;

        private void Awake() {
            switch (id) {
                case 1:
                    GameManager.WallhuggerController1 = this;
                    break;
                case 2:
                    GameManager.WallhuggerController2 = this;
                    break;
            }
            
            _agent = GetComponent<NavMeshAgent>();
            _playerTransform = GameManager.PlayerController.transform;
        }

        private void Update() {
            switch (state) {
                case State.Waiting:
                    Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * maxRoamingDistance;
                    NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxRoamingDistance, 1);
                    Vector3 finalPosition = hit.position;
                    _agent.SetDestination(finalPosition);
                    state = State.Roaming;
                    break;
                case State.Roaming:
                    bool reachedDestination = !_agent.pathPending && !_agent.isOnOffMeshLink &&
                                              (_agent.remainingDistance <= _agent.stoppingDistance ||
                                               _agent.velocity.magnitude < 0.15f);
                    if (reachedDestination) state = State.Waiting;
                    break;
                case State.Hunting:
                    _agent.SetDestination(_playerTransform.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
