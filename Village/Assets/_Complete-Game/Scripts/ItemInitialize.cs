using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static ItemData;
using static UnityEditor.Progress;

public class itemsyoki : MonoBehaviour
{
    [SerializeField] private ItemDataBase ItemDataBase;
    //　アイテム数管理
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ItemDataBase.GetItemLists().Count; i++)
        {
            //　アイテム数を全て0に
            itemkazu.Add(ItemDataBase.GetItemLists()[i], 0);


        }

        //ポーションのみ数を2にする。
        itemkazu[ItemDataBase.GetItemLists()[1]] = 2;

        var a = itemkazu[ItemDataBase.GetItemLists()[0]];
        var b = itemkazu[ItemDataBase.GetItemLists()[1]];
        var c = itemkazu[ItemDataBase.GetItemLists()[2]];

        Debug.Log(a);
        Debug.Log(b);
        Debug.Log(c);



        var d = ItemDataBase.GetItemLists()[1].GetItemtype();
        Debug.Log(d);


    }

}