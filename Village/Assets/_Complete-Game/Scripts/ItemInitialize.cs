using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static ItemData;
using static UnityEditor.Progress;

public class itemsyoki : MonoBehaviour
{
    [SerializeField] private ItemDataBase ItemDataBase;
    //�@�A�C�e�����Ǘ�
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ItemDataBase.GetItemLists().Count; i++)
        {
            //�@�A�C�e������S��0��
            itemkazu.Add(ItemDataBase.GetItemLists()[i], 0);


        }

        //�|�[�V�����̂ݐ���2�ɂ���B
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