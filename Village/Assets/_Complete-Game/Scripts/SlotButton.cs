using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SlotButton : MonoBehaviour,




    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{

    [SerializeField] GameObject itemObject;

    itemkanri itemscript;

    void Start()
    {
        itemscript = itemObject.GetComponent<itemkanri>();

    }


    // 押す  
    public void OnPointerClick(PointerEventData eventData)
    {

        itemscript.slotkoushin();

    }
    // 押されたまま
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    // 押した後放した 
    public void OnPointerUp(PointerEventData eventData)
    {

    }
}