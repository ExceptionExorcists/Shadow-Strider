using UnityEngine;
using UnityEngine.Events;

namespace Script {
    public class Window : MonoBehaviour {
        public GameObject target;
        public Color interactionRangeColor;
        public Color defaultRangeColor;
        public UnityEvent openEvent;
        public UnityEvent closeEvent;

        private bool inRange;
        private bool isOpen = true;
        Animator anim;

        private void Start() {
            gameObject.GetComponent<Outline>().OutlineColor = defaultRangeColor;
            anim = GetComponent<Animator>();
        }

        private void Update() {
            if (!Input.GetKeyDown(KeyCode.E) || !inRange || target is null) return;

            if (isOpen)
            {
                //close window
                closeEvent.Invoke();
                anim.SetTrigger("Close");
                isOpen = false;
            }
            else
            {
                //open window
                openEvent.Invoke();
                anim.SetTrigger("Open");
                isOpen = true;
            }


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
