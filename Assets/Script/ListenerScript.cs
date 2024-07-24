using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ListenerScript : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public GameObject target;
    private States _state;
    public float maxRoamingDistance;
    
    private enum States { Waiting, Roaming, Investigating, Hunting }
    
    public enum NoiseStrength { Low, Medium, High }

    private Transform _roamingTowards;
    // Start is called before the first frame update
    private void Start()
    {
        _state = States.Waiting;
    }

    // Update is called once per frame
    private void Update()
    {
        //click to direct enemy, for testing
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        switch (_state) {
            case States.Waiting:
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
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Debug.Log("State: " + _state);
    }

    public void InvestigateArea(Vector3 position, GameObject gameObject, float radius, NoiseStrength strength) {
        if (_state == States.Hunting) return;
        
        float distance = Vector3.Distance(position, transform.position);
        if (distance > radius) return;

        switch (strength) {
            case NoiseStrength.Low:
                break;
            case NoiseStrength.Medium:
                _state = States.Investigating;
                agent.SetDestination(position);
                break;
            case NoiseStrength.High:
                _state = States.Hunting;
                target = gameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strength), strength, null);
        }

    }
}
