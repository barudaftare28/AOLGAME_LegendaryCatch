using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    // object data ikan
    public FishData _fishData;
    // untuk tampilan ikan
    [SerializeField] Image _iconImage;
    // Start is called once be  fore the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // public void setItem (FishData data)
    // {
    //     _fishData = data;
    //     _iconImage.sprite = _fishData.fishIcon;
    //     _iconImage.enabled = true;
    // }
    // Update is called once per frame
    void Update()
    {
        _iconImage.sprite = _fishData.fishIcon;
    }
}
