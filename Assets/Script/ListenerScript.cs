using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ListenerScript : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    States state;

    public float maxRoamingDistance;

    public float attenuationRate;
    public float huntNoise;
    public float alertNoise;

    public float baseSpeed;
    public float speedIncreaseAmount;


    enum States {waiting, roaming, investigating, hunting}

    private Transform roamingTowards;
    // Start is called before the first frame update
    void Start()
    {
        state = States.waiting;
    }

    // Update is called once per frame
    void Update()
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

        //Roaming
        if(state == States.waiting)
        {
            Debug.Log("roaming");
            //get random location
            Vector3 randomDirection = Random.insideUnitSphere * maxRoamingDistance;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, maxRoamingDistance, 1);
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
            state = States.roaming;
        }
        if(state == States.roaming)
        {
            if(agent.velocity.magnitude < 0.15f)
            {
                state = States.waiting;
            }
        }
        if(state == States.investigating)
        {


            if(agent.velocity.magnitude < 0.15f)
            {
                state = States.waiting;
            }
        }

        Debug.Log("State: " + state);
    }

    public void InvestigateArea(Vector3 position, float strength)
    {
        float distance = Vector3.Distance(position, transform.position);
        float noiseStrength = strength - attenuationRate * distance;
        noiseStrength = Mathf.Clamp(noiseStrength, 0, noiseStrength);

        state = States.investigating;
        agent.SetDestination(position);

    }

}
