using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableObject : MonoBehaviour
{
    private bool inRange = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRange)
        {
            Debug.Log("button pressed");
        }

    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("button:" +other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<Outline>().enabled = true;
            inRange = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<Outline>().enabled = false;
            inRange = false;
        }
    }
}
