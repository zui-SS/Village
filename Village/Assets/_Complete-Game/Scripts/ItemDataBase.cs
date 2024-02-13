using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Data/Create ItemDataBase")]
public class ItemDataBase : ScriptableObject
{

    [SerializeField]
    private List<ItemData> itemLists = new List<ItemData>();

    public enum E_ITEM_TYPE {
        E_NONE = 0,
        E_PATTION,
        E_ROTION, 
        E_PORTION,
        E_FOOD,
        E_DRINK,
        E_NUM
    }

    //　アイテムリストを返す
    public List<ItemData> GetItemLists()
    {
        return itemLists;
    }

}