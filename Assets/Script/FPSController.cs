using UnityEngine;

namespace Script {
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour {
        public GameObject listener;
        private ListenerScript _listenerScript;
        public Camera playerCamera;
        public float walkSpeed = 6.0f;
        private Vector3 _moveDirection = Vector3.zero;
        private float _rotationX;
        public bool canMove = true;
        private CharacterController _characterController;
        private Vector3 _initialCameraPosition;
        
        
        // Start is called before the first frame update
        private void Start() {
            _characterController = GetComponent<CharacterController>();
            _initialCameraPosition = playerCamera.transform.position;
            _listenerScript = listener.GetComponent<ListenerScript>();
        }

        // Update is called once per frame
        private void Update() {
            Move();

            Rotation();

            UpdateCameraPosition();
        }

        private void Move() {
            Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
            Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);

            float curSpeedX = canMove ? walkSpeed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? walkSpeed * Input.GetAxis("Horizontal") : 0;
            
            _moveDirection = forward * curSpeedX + right * curSpeedY;
            
            _moveDirection.y = 0.0f;

            _characterController.Move(_moveDirection * Time.deltaTime);
            
            if (_moveDirection != Vector3.zero) _listenerScript.InvestigateArea(transform.position, gameObject, 10.0f, ListenerScript.NoiseStrength.Medium);
        }

        private void Rotation() {
            Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (!groundPlane.Raycast(cameraRay, out float rayLength)) return;
            
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        private void UpdateCameraPosition() {
            playerCamera.transform.position = _initialCameraPosition + transform.position;
        }
    }
}
