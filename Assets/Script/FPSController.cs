using UnityEngine;

namespace Script {
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour {
        public Camera playerCamera;
        public float walkSpeed = 6.0f;
        public float runSpeed = 12.0f;
        public float gravity = 10.0f;
        private Vector3 _moveDirection = Vector3.zero;
        private float _rotationX;
        public bool canMove = true;
        private CharacterController _characterController;
        private Vector3 _intialCameraPosition;
        
        
        // Start is called before the first frame update
        private void Start() {
            _characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _intialCameraPosition = playerCamera.transform.position;
        }

        // Update is called once per frame
        private void Update() {
            #region Handles Movement
            
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            
            _moveDirection = forward * curSpeedX + right * curSpeedY;
            
            #endregion

            #region Handles Jumping

            if (!_characterController.isGrounded) {
                _moveDirection.y -= gravity * Time.deltaTime;
            }

            #endregion

            #region Handles Rotation

            _characterController.Move(_moveDirection * Time.deltaTime);

            if (!canMove) return;

            Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(cameraRay, out float rayLength)) {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);

                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }

            #endregion
            
            playerCamera.transform.position = _intialCameraPosition + transform.position;
        }
    }
}
