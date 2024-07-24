using UnityEngine;

namespace Script {
    public class CameraController : MonoBehaviour {
        private Transform _target;
        private Vector3 _initialPosition;

        public void Start() {
            _target = GameManager.PlayerController.transform;
            _initialPosition = transform.position;
        }

        private void LateUpdate() {
            transform.position = _initialPosition + _target.position;
        }
    }
}
