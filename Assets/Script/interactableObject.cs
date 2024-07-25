using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class interactableObject : MonoBehaviour
{
    public enum Items { empty, firecracker, glowstick }

    public UnityEvent interationEvent;
    public bool isInteractable = true;
    public bool isItem;
    private bool inRange = false;
    public Color interactionRangeColor;
    public Color defaultRangeColor;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Outline>().OutlineColor = defaultRangeColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRange && isInteractable)
        {
            interationEvent.Invoke();
            if (isItem)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("interaction range:" +other.gameObject.tag);
        if(other.gameObject.tag == "Player" && isInteractable)
        {
            gameObject.GetComponent<Outline>().OutlineColor = interactionRangeColor;
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<Outline>().OutlineColor= defaultRangeColor;
            inRange = false;
        }
    }
}
