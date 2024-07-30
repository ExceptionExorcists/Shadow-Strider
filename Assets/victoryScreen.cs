using System.Collections.Generic;
using UnityEngine;

public class victoryScreen : MonoBehaviour {
    public GameObject victoryText;
    public List<GameObject> toDisable = new();

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")) return;
        
        victoryText.SetActive(true);
        AudioListener.volume = 0.0f;
        
        foreach (var thing in toDisable) thing.SetActive(false);
    }

}
