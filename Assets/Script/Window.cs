using UnityEngine;

namespace Script {
    public class Window : MonoBehaviour {
        public GameObject target;
        public Color interactionRangeColor;
        public Color defaultRangeColor;
        
        private bool inRange;
        
        private void Start() {
            gameObject.GetComponent<Outline>().OutlineColor = defaultRangeColor;
        }

        private void Update() {
            if (!Input.GetKeyDown(KeyCode.E) || !inRange || target is null) return;
            
            target.SetActive(!target.activeSelf);
        }

        private void OnTriggerStay(Collider other) {
            if (!other.gameObject.CompareTag("Player")) return;
            
            gameObject.GetComponent<Outline>().OutlineColor = interactionRangeColor;
            inRange = true;
        }

        private void OnTriggerExit(Collider other) {
            if (!other.gameObject.CompareTag("Player")) return;
            
            gameObject.GetComponent<Outline>().OutlineColor = defaultRangeColor;
            inRange = false;
        }
    }
}
