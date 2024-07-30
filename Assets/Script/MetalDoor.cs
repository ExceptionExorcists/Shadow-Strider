using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDoor : MonoBehaviour {
    private bool _powered = true;
    public interactableObject interactable;

    public void Toggle() {
        if (!_powered) return;
        
        gameObject.SetActive(false);
    }

    public void TogglePower() {
        _powered = !_powered;

        if (_powered) {
            interactable.interactionRangeColor = Color.yellow;
        } else {
            interactable.interactionRangeColor = Color.red;
        }
    }
}
