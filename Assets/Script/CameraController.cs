using System;
using UnityEngine;

namespace Script {
    public class CameraController : MonoBehaviour {
        private GameObject _targetGameObject;
        private Transform _targetTransform;
        private Vector3 _initialPosition;
        private MeshFader _meshFader;

        private void Start() {
            _targetGameObject = GameManager.PlayerController.gameObject;
            _targetTransform = GameManager.PlayerController.transform;
            _initialPosition = transform.position;
        }

        private void Update() {
            Vector3 direction = _targetTransform.position - transform.position;
            Ray ray = new Ray(transform.position, direction);

            if (!Physics.Raycast(ray, out RaycastHit hit) || hit.collider is null) return;
            
            if (hit.collider.gameObject == _targetGameObject) {
                if (_meshFader is not null) _meshFader.shouldBeVisible = true;
            } else {
                _meshFader = hit.collider.GetComponent<MeshFader>();
                
                if (_meshFader is not null) _meshFader.shouldBeVisible = false;
            }
        }

        private void LateUpdate() {
            transform.position = _initialPosition + _targetTransform.position;
        }
    }
}
