using UnityEngine;

namespace Script {
    public class Damage : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (!other.gameObject.CompareTag("Player")) return;
            
            GameManager.Reset();
        }
    }
}
