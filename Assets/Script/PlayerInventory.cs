using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject listener;

    //inventory
    public static int currSlot = 0;
    public GameObject firecrackPrefab;
    public GameObject glowstickPrefab;
    public enum Items { empty, firecracker, glowstick }
    [SerializeField]
    public static List<Items> inventory;
    public List<Items> inv;
    public float throwForce;


    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<Items> { Items.empty, Items.empty };

    }

    // Update is called once per frame
    void Update()
    {
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
        if (inventory[currSlot] != Items.empty)
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
