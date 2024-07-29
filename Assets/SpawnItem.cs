using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public List<GameObject> Items;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Items.Count);
        int item = Random.Range(0, Items.Count);
        GameObject spawnedItem = Instantiate(Items[item]);
        spawnedItem.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
