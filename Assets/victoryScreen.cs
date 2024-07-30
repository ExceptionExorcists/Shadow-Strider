using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class victoryScreen : MonoBehaviour {
    public GameObject victoryText;
    public List<GameObject> toDisable = new();

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")) return;
        
        victoryText.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioListener>().enabled = false;
        
        foreach (var thing in toDisable) thing.SetActive(false);
    }

}
