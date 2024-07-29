using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private static Image invSlot1;
    private static Image invSlot2;
    public Sprite glowStickSprite;
    public Sprite firecrackerSprite;
    public Color transparentInv;
    public Color visibleInv;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<Items> { Items.empty, Items.empty };
        GameObject invUI = GameObject.Find("WelcomeScreen");
        invSlot1 = invUI.transform.Find("item_slot1").gameObject.GetComponent<Image>();
        invSlot2 = invUI.transform.Find("item_slot2").gameObject.GetComponent<Image>();
        Debug.Log(invSlot1);
        Debug.Log(invSlot2);
        UpdateInventoryUI();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            swapSlot();
            UpdateInventoryUI();
        }
        //Use item
        if (Input.GetMouseButtonDown(0))
        {
            UseItem();
        }
        inv = inventory;
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
        UpdateInventoryUI();
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
        UpdateInventoryUI();
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
            swapSlot();
        }
        UpdateInventoryUI();
    }

    public GameObject ThrowItem(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.GetComponent<interactableObject>().isInteractable = false;
        obj.transform.position = transform.position;
        obj.GetComponent<Outline>().enabled = false;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = throwForce * transform.forward;
        UpdateInventoryUI();

        return obj;
    }

    public void UpdateInventoryUI()
    {
        switch (inventory[currSlot])
        {
            case Items.empty:
                invSlot1.sprite = null;
                invSlot1.color = transparentInv;
                break;
            case Items.firecracker:
                invSlot1.sprite = firecrackerSprite;
                invSlot1.color = visibleInv;
                break;
            case Items.glowstick:
                invSlot1.sprite = glowStickSprite;
                invSlot1.color = visibleInv;
                break;
        }
        switch(inventory[Mathf.Abs(currSlot - 1)])
        {
            
            case Items.empty:    
                invSlot2.sprite = null;
                invSlot2.color = transparentInv;
                break;
            case Items.firecracker:
                invSlot2.sprite = firecrackerSprite;
                invSlot2.color = visibleInv;
                break;
            case Items.glowstick:
                invSlot2.sprite = glowStickSprite;
                invSlot2.color = visibleInv;
                break;
        }
    }

    public void swapSlot()
    {
        switch (currSlot)
        {
            case 0:
                currSlot = 1;
                break;
            case 1:
                currSlot = 0;
                break;
        }

    }
}
