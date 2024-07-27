using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class documentScript : MonoBehaviour
{
    private GameObject player;
    public float disableTextDistance;
    public TMP_Text textfield;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        player = players[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) > disableTextDistance)
        {
            textfield.enabled = false;
        }
    }
}
