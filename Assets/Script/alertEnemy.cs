using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alertEnemy : MonoBehaviour
{
    public GameObject listener;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("alert!");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("alert!");
            listener.GetComponent<ListenerScript>().InvestigateArea(transform.position, gameObject, ListenerScript.NoiseStrength.High);
        }
    }
}
