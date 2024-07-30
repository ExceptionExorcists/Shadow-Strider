using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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

        public AudioClip stepClip;
        private AudioSource AS;
        public float defaultClipDuration;
        private float timer = 0;
        private int _areaMask = -1;
        private float _secondsDelay = 0.1f;

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

        private void Start()
        {
            AS = GetComponent<AudioSource>();
            //_areaMask = NavMesh.GetAreaFromName("Climbable");
        }

        private void Update() {
            float stepClipDuration;
            if(state == State.Roaming)
            {
                stepClipDuration = defaultClipDuration;
            }else if(state == State.Hunting)
            {
                stepClipDuration = defaultClipDuration / 2;
            }
            else
            {
                stepClipDuration = defaultClipDuration;
            }
            
            timer += Time.deltaTime;
            if (timer > stepClipDuration)
            {
                AS.pitch = Random.Range(0.8f, 1.2f);
                AS.Play();
                timer = 0;
            }

            switch (state) {
                case State.Waiting:
                    Vector3 randomDirection = Random.insideUnitSphere * maxRoamingDistance + transform.position;
                    if (!NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxRoamingDistance, _areaMask)) return;
                    
                    _agent.SetDestination(hit.position);
                    state = State.Roaming;
                    break;
                case State.Roaming:
                    bool reachedDestination = !_agent.pathPending && !_agent.isOnOffMeshLink &&
                                              (_agent.remainingDistance <= _agent.stoppingDistance ||
                                               _agent.velocity.magnitude < 0.15f);
                    if (reachedDestination) state = State.Waiting;
                    break;
                case State.Hunting:
                    if (_secondsDelay <= 0.0f) {
                        _agent.SetDestination(_playerTransform.position);
                        _secondsDelay = 0.1f;
                    } else {
                        _secondsDelay -= Time.deltaTime;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
