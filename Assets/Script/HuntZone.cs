using UnityEngine;

namespace Script {
    public class HuntZone : MonoBehaviour {
        private WallhuggerController _wallhuggerController;

        private void Awake() {
            _wallhuggerController = GameManager.WallhuggerController;
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.gameObject.CompareTag("Player")) return;

            _wallhuggerController.state = WallhuggerController.State.Hunting;
        }

        private void OnTriggerExit(Collider other) {
            if (!other.gameObject.CompareTag("Player")) return;

            _wallhuggerController.state = WallhuggerController.State.Waiting;
        }
    }
}
