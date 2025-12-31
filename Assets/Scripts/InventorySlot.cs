using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour
{
    // item yang ada di slot
    public GameObject _heldItems;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    //ganti item yang ada di slot
    public void setHeldItem(GameObject item)
    {
        _heldItems = item;
        _heldItems.transform.position = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
