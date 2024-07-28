using UnityEngine;

namespace Script {
    public class HuntZone : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (!other.gameObject.CompareTag("Player")) return;

            GameManager.WallhuggerController1.state = WallhuggerController.State.Hunting;
            GameManager.WallhuggerController2.state = WallhuggerController.State.Hunting;
        }

        private void OnTriggerStay(Collider other) {
            if (!other.gameObject.CompareTag("Player")) return;

            GameManager.WallhuggerController1.state = WallhuggerController.State.Hunting;
            GameManager.WallhuggerController2.state = WallhuggerController.State.Hunting;
        }

        private void OnTriggerExit(Collider other) {
            if (!other.gameObject.CompareTag("Player")) return;

            GameManager.WallhuggerController1.state = WallhuggerController.State.Waiting;
            GameManager.WallhuggerController2.state = WallhuggerController.State.Waiting;
        }
    }
}
