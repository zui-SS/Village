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


    // ����  
    public void OnPointerClick(PointerEventData eventData)
    {

        itemscript.slotkoushin();

    }
    // �����ꂽ�܂�
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    // ������������� 
    public void OnPointerUp(PointerEventData eventData)
    {

    }
}