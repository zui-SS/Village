using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class ItemButton : MonoBehaviour,

    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{

    [SerializeField] GameObject ActiveObject;
    [SerializeField] GameObject itemkanri;
    itemkanri itemscript;




    void Start()
    {
        itemscript = itemkanri.GetComponent<itemkanri>();

    }

    // 押す  
    public void OnPointerClick(PointerEventData eventData)
    {


        ActiveObject.gameObject.SetActive(true);
        itemscript.slotkoushin();

        // アイテム更新.
        itemscript.Motimonokoushin();

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
