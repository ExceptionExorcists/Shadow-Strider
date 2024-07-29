using UnityEngine;

namespace Script {
    public class MetalDetector : MonoBehaviour {
        private bool _powered = true;
        private bool _detected;

        private void OnTriggerEnter(Collider other) {
            if (!_powered || _detected) return;

            _detected = true;
            GameManager.ListenerScript.EnterHuntingState(transform.position, GameManager.PlayerController.gameObject);
        }

        public void TogglePower() {
            _powered = !_powered;
            _detected = false;
        }
    }
}
