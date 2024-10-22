using UnityEngine;
using UnityEngine.AI;

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

        private Light flashLight;
        private bool isListenerHunting;
        public float lightFlickerDuration;
        private float defaultLightIntensity;
        private float counter;
        private bool inFlickerLow = false;

        private PlayerController() {
            GameManager.PlayerController = this;
        }
        
        private void Start() {
            _camera = Camera.main;
            _characterController = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            _listenerScript = listener.GetComponent<ListenerScript>();
            AS = GetComponent<AudioSource>();
            flashLight = transform.Find("Flashlight").gameObject.GetComponent<Light>();
            defaultLightIntensity = flashLight.intensity;
        }

        private void Update() {
            UpdatePosition();
            UpdateRotation();

            isListenerHunting = (GameManager.ListenerScript._state == ListenerScript.States.Hunting);
            if (isListenerHunting)
            {
                flashLight.GetComponent<NavMeshObstacle>().enabled = false;
                counter += Time.deltaTime;
                if(counter > lightFlickerDuration && !inFlickerLow)
                {
                    flashLight.intensity = Random.Range(defaultLightIntensity / 2, defaultLightIntensity);
                    counter = 0;
                    inFlickerLow = true;
                }
                else if(counter > lightFlickerDuration && inFlickerLow)
                {
                    counter = 0;
                    inFlickerLow = false;
                }
            }
            else
            {
                flashLight.GetComponent<NavMeshObstacle>().enabled = true;
                flashLight.intensity = defaultLightIntensity;
            }
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
            moveDirection.y -= 1.0f;
            if(moveDirection.x != 0.0f || moveDirection.z != 0.0f)
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
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            timer += Time.deltaTime;

            if (moveDirection.x != 0.0f || moveDirection.z != 0.0f)
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
