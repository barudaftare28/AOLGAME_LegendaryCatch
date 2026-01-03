using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using Mono.Cecil;
using System;
public class InventoryManager : MonoBehaviour,IPointerDownHandler,IPointerUpHandler  //interface buat drag mouse
{
    public GameObject _draggedObject; //container buat item yang di drag
    public GameObject _lastObject; //container buat track item terakhir yang di click sekaligus untuk nukar item yang didrag 

    public InventorySlot _lastSlot; //container untuk 

    public bool _isOpened = false; //flag status inventory
    // untuk toggle inventory dgn key
    [SerializeField] GameObject _inventoryParent; //utk hide inventory

    [SerializeField] GameObject[] _invSlot = new GameObject[15]; //jumlah slot inventory yang ada ubah ini juga jika ingin ganti max slot inventory

    [SerializeField] GameObject _itemPrefab;

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
                _lastSlot = slot;
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
            } else if (slot == null) //jika item di drop di luar slot maka akan dikembalikan ke posisi semula
            {
                slot = _lastObject.GetComponent<InventorySlot>();
                slot.setHeldItem(_draggedObject);
                _draggedObject = null;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // _inventoryParent.SetActive(_isOpened);
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
// ketika ikan berhasil ditangkap
    public void fishAdded(FishData caughtFish)
    {
        GameObject emptySlot = null;

        // loop untuk cek inventory slot
        for(int i = 0; i < _invSlot.Length; i++)
        {
            InventorySlot slot = _invSlot[i].GetComponent<InventorySlot>();

            // jika kosong masukan 
            if(slot._heldItems == null) 
            {
                emptySlot = _invSlot[i];
                break;
            }
        }

        // cek apakah kosong atau tidak
        if(emptySlot != null) //masukkan ke inventory
        {
            GameObject fishCreated = Instantiate(_itemPrefab);
            fishCreated.GetComponent<InventoryItem>()._fishData = caughtFish;
            fishCreated.transform.SetParent(emptySlot.transform.parent.parent);
            emptySlot.GetComponent<InventorySlot>().setHeldItem(fishCreated);

        } else
        {
            Debug.Log("Inventory Penuh !");
        }
    }
 


}
