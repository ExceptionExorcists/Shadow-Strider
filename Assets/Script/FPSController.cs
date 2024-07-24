using UnityEngine;

namespace Script {
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour {
        public Camera playerCamera;
        public float walkSpeed = 6.0f;
        public float runSpeed = 12.0f;
        public float jumpPower = 7.0f;
        public float gravity = 10.0f;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;
        private Vector3 _moveDirection = Vector3.zero;
        private float _rotationX;
        public bool canMove = true;
        private CharacterController _characterController;
        
        
        // Start is called before the first frame update
        private void Start() {
            _characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        private void Update() {
            #region Handles Movement
            
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = _moveDirection.y;
            
            _moveDirection = forward * curSpeedX + right * curSpeedY;
            
            #endregion

            #region Handles Jumping

            if (Input.GetButton("Jump") && canMove && _characterController.isGrounded) {
                _moveDirection.y = jumpPower;
            } else {
                _moveDirection.y = movementDirectionY;
            }

            if (!_characterController.isGrounded) {
                _moveDirection.y -= gravity * Time.deltaTime;
            }

            #endregion

            #region Handles Rotation

            _characterController.Move(_moveDirection * Time.deltaTime);

            if (!canMove) return;
            
            _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0.0f, 0.0f);
            transform.rotation *= Quaternion.Euler(0.0f, Input.GetAxis("Mouse X") * lookSpeed, 0.0f);

            #endregion
        }
    }
}
