using UnityEngine;

namespace Script {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour {
        public float walkSpeed = 6.0f;
        private Camera _camera;
        
        private CharacterController _characterController;
        private Animator anim;

        private AudioSource AS;
        public AudioClip walkingClip;
        public float clipDuration = 0.2f;
        private float timer;

        private ListenerScript _listenerScript;
        public GameObject listener;

        private PlayerController() {
            GameManager.PlayerController = this;
        }
        
        private void Start() {
            _camera = Camera.main;
            _characterController = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            _listenerScript = listener.GetComponent<ListenerScript>();
            AS = GetComponent<AudioSource>();
        }

        private void Update() {
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition() {
            Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
            forward.y = 0.0f;
            forward.Normalize();
            
            Vector3 right = _camera.transform.TransformDirection(Vector3.right);
            right.y = 0.0f;
            right.Normalize();

            float curSpeedX = walkSpeed * Input.GetAxis("Vertical");
            float curSpeedY = walkSpeed * Input.GetAxis("Horizontal");
            
            Vector3 moveDirection = forward * curSpeedX + right * curSpeedY;
            moveDirection.y = 0.0f;
            if(moveDirection != Vector3.zero)
            {
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }

            bool noMovement = moveDirection == Vector3.zero;
            if (noMovement) return;

            _characterController.Move(moveDirection * Time.deltaTime);
            timer += Time.deltaTime;

            if (moveDirection != Vector3.zero)
            { 
                _listenerScript.InvestigateArea(transform.position, gameObject, ListenerScript.NoiseStrength.Medium);
                if (timer > clipDuration)
                {
                    AS.pitch = Random.Range(0.8f, 1.2f);
                    AS.PlayOneShot(walkingClip);
                    timer = 0;
                }
                
                
            }

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
