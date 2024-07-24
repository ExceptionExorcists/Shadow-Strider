using UnityEngine;

namespace Script {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour {
        public float walkSpeed = 6.0f;
        private Camera _camera;
        
        private CharacterController _characterController;

        private PlayerController() {
            GameManager.PlayerController = this;
        }
        
        private void Start() {
            _camera = Camera.main;
            _characterController = GetComponent<CharacterController>();
        }

        private void Update() {
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition() {
            Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
            Vector3 right = _camera.transform.TransformDirection(Vector3.right);

            float curSpeedX = walkSpeed * Input.GetAxis("Vertical");
            float curSpeedY = walkSpeed * Input.GetAxis("Horizontal");
            
            Vector3 moveDirection = forward * curSpeedX + right * curSpeedY;
            moveDirection.y = 0.0f;

            bool noMovement = moveDirection == Vector3.zero;
            if (noMovement) return;

            _characterController.Move(moveDirection * Time.deltaTime);
        }

        private void UpdateRotation() {
            Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (!groundPlane.Raycast(cameraRay, out float rayLength)) return;
            
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
