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

    // ����  
    public void OnPointerClick(PointerEventData eventData)
    {


        ActiveObject.gameObject.SetActive(true);
        itemscript.slotkoushin();

        // �A�C�e���X�V.
        itemscript.Motimonokoushin();

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
