using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script {
    public class CameraController : MonoBehaviour {
        private Transform _targetTransform;
        private Vector3 _defaultPosition;
        private List<MeshFader> _currentlyFadedObjects = new();
        private int _fadeoutLayerMask;
        private readonly RaycastHit[] RAYCAST_BUFFER = new RaycastHit[20];

        private void Start() {
            _targetTransform = GameManager.PlayerController.transform;
            _defaultPosition = transform.position + new Vector3(45.0f, 0.0f, 0.0f);
            _fadeoutLayerMask = 1 << LayerMask.NameToLayer("Fadeout");
        }

        private void Update() {
            Vector3 direction = _targetTransform.position - transform.position;
            Ray ray = new Ray(transform.position, direction);
            var raycastBufferSize = Physics.RaycastNonAlloc(ray, RAYCAST_BUFFER, Vector3.Distance(transform.position, _targetTransform.position), _fadeoutLayerMask);

            HashSet<MeshFader> hitObjects = new HashSet<MeshFader>();
            for (int i = 0; i < raycastBufferSize; i++) {
                MeshFader fader = RAYCAST_BUFFER[i].collider.GetComponent<MeshFader>();
                if (fader is null) continue;
                
                fader.shouldBeVisible = false;
                hitObjects.Add(fader);
            }

            foreach (var fader in _currentlyFadedObjects.Where(fader => !hitObjects.Contains(fader))) {
                fader.shouldBeVisible = true;
            }

            _currentlyFadedObjects = new List<MeshFader>(hitObjects);
        }

        private void LateUpdate() {
            transform.position = _defaultPosition + _targetTransform.position;
        }
    }
}
