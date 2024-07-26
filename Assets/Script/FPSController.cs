using UnityEngine;
using System.Collections.Generic;

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
   
        //inventory
        public static int currSlot = 0;
        public GameObject firecrackPrefab;
        public GameObject glowstickPrefab;
        public enum Items {empty, firecracker, glowstick}
        [SerializeField]
        public static List<Items> inventory;
        public List<Items> inv;
        public float throwForce;

        // Start is called before the first frame update
        private void Start() {
            inventory = new List<Items> { Items.empty, Items.empty };
            _characterController = GetComponent<CharacterController>();
            _initialCameraPosition = playerCamera.transform.position;
            _listenerScript = listener.GetComponent<ListenerScript>();
        }

        // Update is called once per frame
        private void Update() {
            inv = inventory;
            Move();

            Rotation();

            UpdateCameraPosition();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                switch (currSlot)
                {
                    case 0:
                        currSlot = 1;
                        //swap UI elements
                        break;
                    case 1:
                        currSlot = 0;
                        //swap UI elements
                        break;
                }
            }
            //Use item
            if (Input.GetMouseButtonDown(0))
            {
                UseItem();
            }
        }

        private void Move() {
            Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
            Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);

            float curSpeedX = canMove ? walkSpeed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? walkSpeed * Input.GetAxis("Horizontal") : 0;
            
            _moveDirection = forward * curSpeedX + right * curSpeedY;
            
            _moveDirection.y = 0.0f;

            _characterController.Move(_moveDirection * Time.deltaTime);
            
            if (_moveDirection != Vector3.zero) _listenerScript.InvestigateArea(transform.position, gameObject, ListenerScript.NoiseStrength.Medium);
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

        public void GetItem(string item)
        {
            var newItem = item switch
            {
                "glowstick" => Items.glowstick,
                "firecracker" => Items.firecracker,
                _ => Items.empty,
            };

            if (inventory[currSlot] == Items.empty)
            {
                inventory[currSlot] = newItem;
            }
            else if (inventory[(int)Mathf.Abs(currSlot - 1)] == Items.empty)
            {
                inventory[(int)Mathf.Abs(currSlot - 1)] = newItem;
            }
            else 
            {
                DropItem(inventory[currSlot]);
                inventory[currSlot] = newItem;
            }
        }

        public void DropItem(Items item)
        {
            switch (item)
            {
                case Items.firecracker:
                    GameObject obj_f = Instantiate(firecrackPrefab);
                    obj_f.transform.position = transform.position;
                    inventory[currSlot] = Items.empty;
                    break;

                case Items.glowstick:
                    GameObject obj_g = Instantiate(glowstickPrefab);
                    obj_g.transform.position = transform.position;
                    inventory[currSlot] = Items.empty;
                    break;

                default:
                    break;
            }
        }

        public void UseItem()
        {
            if(inventory[currSlot] != Items.empty)
            {
                switch (inventory[currSlot])
                {
                    case Items.firecracker:
                        inventory[currSlot] = Items.empty;
                        GameObject fckr = ThrowItem(firecrackPrefab);
                        fckr.GetComponent<firecrackerScript>().UseFireCracker();
                        break;

                    case Items.glowstick:
                        inventory[currSlot] = Items.empty;
                        GameObject obj = ThrowItem(glowstickPrefab);
                        obj.GetComponent<glowstickScript>().used = true;
                        GameObject light = obj.transform.GetChild(0).gameObject;
                        light.GetComponent<Light>().enabled = true;
                        break;
                }
            }
        }

        public GameObject ThrowItem(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab);
            obj.GetComponent<interactableObject>().isInteractable = false;
            obj.transform.position = transform.position;
            obj.GetComponent<Outline>().enabled = false;
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = throwForce * transform.forward;

            return obj;
        }


    }
}
