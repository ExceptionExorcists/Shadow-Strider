using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ListenerScript : MonoBehaviour
{
    public Camera cam;
    private NavMeshAgent agent;
    public GameObject target;
    public float huntingSpeed;
    public float defaultSpeed;
    private States _state;
    public float maxRoamingDistance;
    public float huntingRange;
    public float investigateRange;
    private enum States { Waiting, Roaming, Investigating, Hunting }
    
    public enum NoiseStrength { Low, Medium, High }

    private Transform _roamingTowards;
    private Animator animator;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        _state = States.Waiting;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        //click to direct enemy, for testing
        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }*/

        switch (_state) {
            case States.Waiting:
                agent.speed = defaultSpeed;
                Vector3 randomDirection = Random.insideUnitSphere * maxRoamingDistance;
                NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxRoamingDistance, 1);
                Vector3 finalPosition = hit.position;
                agent.SetDestination(finalPosition);
                _state = States.Roaming;
                break;
            case States.Roaming:
                if(agent.velocity.magnitude < 0.15f) _state = States.Waiting;
                break;
            case States.Investigating:
                if(agent.velocity.magnitude < 0.15f) _state = States.Waiting;
                break;
            case States.Hunting:
                agent.SetDestination(target.transform.position);
                if (agent.velocity.magnitude < 0.15f)
                {
                    _state = States.Waiting;
                    animator.SetBool("Hunting", false);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Debug.Log("State: " + _state);
    }

    public void InvestigateArea(Vector3 position, GameObject huntTarget, NoiseStrength strength) {
        if (_state == States.Hunting) return;

        float radius = Vector3.Distance(transform.position, position);

        switch (strength) {
            case NoiseStrength.Low:
                break;
            case NoiseStrength.Medium:
                if(radius < huntingRange)
                {
                    _state = States.Hunting;
                    EnterHuntingState(position, huntTarget);
                }
                else
                {
                    if(radius < investigateRange)
                    {
                        _state = States.Investigating;
                        agent.SetDestination(position);
                    }
                }
                break;
            case NoiseStrength.High:
                EnterHuntingState(position, huntTarget);                
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strength), strength, null);
        }

    }

    public void EnterHuntingState(Vector3 position, GameObject huntTarget)
    {
        animator.SetBool("Hunting", true);
           _state = States.Hunting;
        agent.SetDestination(position);
        target = huntTarget;
        agent.speed = huntingSpeed;
    }
}
