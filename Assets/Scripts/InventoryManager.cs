using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mono.Cecil;
using System;
public class InventoryManager : MonoBehaviour,IPointerDownHandler,IPointerUpHandler  //interface buat drag mouse
{
    public GameObject _draggedObject; //container buat item yang di drag
    public GameObject _lastObject; //container buat track item terakhir yang di click sekaligus untuk nukar item yang didrag 

    public bool _isOpened = false; //flag status inventory
    // untuk toggle inventory dgn key
    [SerializeField] GameObject _inventoryParent; //utk hide inventory

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // ketika mouse klik 
    public void OnPointerDown(PointerEventData eventData)
    {
    
       if (eventData.button == PointerEventData.InputButton.Left)
       {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject; //game object yg di klik  
            InventorySlot slot = clickedObject.GetComponent<InventorySlot>(); // slot yang dklik
            //bikin item di slot agar diambil dan bisa di drag
            if (slot != null) 
            {
                _draggedObject = slot._heldItems; 
                slot._heldItems = null;
                _lastObject = clickedObject;
            }
        }
    }
    // jika klik lepas
    public void OnPointerUp(PointerEventData eventData)
    {
        if(_draggedObject != null && eventData.pointerCurrentRaycast.gameObject != null && eventData.button == PointerEventData.InputButton.Left) //cek 1. item sedang di drag, 2. ada item yang dklik, 3. tombol klik mouse harus kiri
        {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
            InventorySlot slot = clickedObject.GetComponent<InventorySlot>();

            //bikin item yang sedang di drag ditaruh di slot 
            if(slot != null && slot._heldItems == null)
            {
                slot.setHeldItem(_draggedObject);
                _draggedObject = null;
            }
            // tuker item yang didrag dengan yang ada di slot
            else if (slot != null && slot._heldItems != null){
               _lastObject.GetComponent<InventorySlot>().setHeldItem(slot._heldItems); 
               slot.setHeldItem(_draggedObject);
               _draggedObject = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _inventoryParent.SetActive(_isOpened);
        // buat bikin item ikutin pointer mouse, kesan nya di drag
        if (_draggedObject != null)
        {
            _draggedObject.transform.position = Input.mousePosition;
        }

        // key input untuk buka inventory (ganti KeyCode kalau mau ubah)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isOpened)
            {
                _isOpened = false;
            } else
            {
                _isOpened = true;
            }
        }
    }
}
