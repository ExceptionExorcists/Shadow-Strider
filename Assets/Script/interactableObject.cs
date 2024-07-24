using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class interactableObject : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteraction;

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
            onInteraction.Invoke();
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
