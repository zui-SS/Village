using Completed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine.UI;
using static ItemData;
using static ItemDataBase;
using static UnityEditor.Progress;



public class itemkanri : MonoBehaviour
{
    [SerializeField] private ItemDataBase ItemDataBase;
    [SerializeField] private Player player;


    //作った各アイテムスロットのiconを指定(スロットの番号順に)
    [SerializeField] GameObject icon1;
    [SerializeField] GameObject icon2;
    [SerializeField] GameObject icon3;
    [SerializeField] GameObject icon4;
    [SerializeField] GameObject icon5;
    [SerializeField] GameObject icon6;
    [SerializeField] GameObject icon7;
    [SerializeField] GameObject icon8;
    [SerializeField] GameObject icon9;
    [SerializeField] GameObject icon10;

    //トグルグループであるinventoryを指定。
    [SerializeField] ToggleGroup togglegroup;

    //アイテム説明表示欄
    //説明欄のTextMeshProを指定。
    [SerializeField] TextMeshProUGUI itemname;

    [SerializeField] TextMeshProUGUI itemsetumei;


    //　アイテム数管理
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();

    //持ち物管理
    List<ItemData> MotimonoList = new List<ItemData>();


    //アイコン管理の配列
    Image[] Icons = new Image[10];


    // Start is called before the first frame update
    void Start()
    {
        //初期化アイテム処理

        for (int i = 0; i < ItemDataBase.GetItemLists().Count; i++)
        {
            //　アイテム数を全て0に
            itemkazu.Add(ItemDataBase.GetItemLists()[i], 0);
        }


        //持っている初期アイテム設定。

        //ポーションの数を2にする。
        itemkazu[ItemDataBase.GetItemLists()[1]] = 2;
        //ロングソードの数を1にする。
        itemkazu[ItemDataBase.GetItemLists()[0]] = 1;

        //持ち物更新処理を呼び出す。
        Motimonokoushin();


    }


    //どこからでもアクセス可能。返り値なし。
    public void Motimonokoushin()
    {

        //持ち物更新処理

        //持ち物リストのクリア
        MotimonoList.Clear();

        //持っている個数が1個以上のアイテムを持ち物リストに追加する。
        for (int i = 0; i < itemkazu.Count; i++)
        {
            var e = itemkazu[ItemDataBase.GetItemLists()[i]];

            if (e > 0)
            {
                MotimonoList.Add(ItemDataBase.GetItemLists()[i]);
            }
        }

        //アイテムスロットのアイコンimageをGetComponentしてアイコン配列に代入。
        Icons[0] = icon1.GetComponent<Image>();
        Icons[1] = icon2.GetComponent<Image>();
        Icons[2] = icon3.GetComponent<Image>();
        Icons[3] = icon4.GetComponent<Image>();
        Icons[4] = icon5.GetComponent<Image>();
        Icons[5] = icon6.GetComponent<Image>();
        Icons[6] = icon7.GetComponent<Image>();
        Icons[7] = icon8.GetComponent<Image>();
        Icons[8] = icon9.GetComponent<Image>();
        Icons[9] = icon10.GetComponent<Image>();

        //各アイコンの画像をnullにする(いったん全て空スロットとする)。

        Icons[0].sprite = null;
        Icons[1].sprite = null;
        Icons[2].sprite = null;
        Icons[3].sprite = null;
        Icons[4].sprite = null;
        Icons[5].sprite = null;
        Icons[6].sprite = null;
        Icons[7].sprite = null;
        Icons[8].sprite = null;
        Icons[9].sprite = null;

        //各アイコンのカラー設定。以下の数値だと灰色のような色となる(空スロット表現用)

        Icons[0].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[1].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[2].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[3].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[4].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[5].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[6].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[7].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[8].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[9].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);

        //持ち物リストの要素数だけ繰り返す。
        for (int i = 0; i < MotimonoList.Count; i++)
        {
            //持ち物リストのi番目をfに代入。
            var f = MotimonoList[i];
            //fのアイコンを配列に代入。
            Icons[i].sprite = f.GetItemicon();
            //配列のアイコンのカラーを白色にする(アイコンを見やすくするため)。
            Icons[i].color = new Color(1, 1, 1, 1);
        }
    }


    public void slotkoushin()
    {
        //スロット更新処理
        //選択されているトグル(つまり選択されているアイテムスロット)を代入。
        Toggle tgl = togglegroup.ActiveToggles().FirstOrDefault();

        //選択されているトグルのゲームオブジェクト名を代入。
        string x = tgl.name;

        //Parseにより文字列の数字をint型やfloat型にできる。今回はint型にしてyに代入。
        int y = int.Parse(x);


        //持ち物リストの要素数がy以上かどうかを確認する。
        if (MotimonoList.Count >= y)
        {
            //選択されているトグルはアイコン表示されている。

            //持ち物リストのy-1番(リストが始まるのが0番からであるため)の名前と個数をZ、Kに代入。
            string z = MotimonoList[y - 1].GetItemname();

            int k = itemkazu[MotimonoList[y - 1]];


            //ZとKを合わせてアイテム名の文字列を作成しtextに出す。
            itemname.text = ($"{z}×{k}");

            //持ち物リストのアイテムの説明をjに代入するしtextに出す。
            string j = MotimonoList[y - 1].GetItemexplanation();
            itemsetumei.text = j;
        }
        else
        {
            //条件に当てはまらなかった場合選択されたアイテムスロットが空だったということ。よってtextにはnullを代入する。
            itemname.text = null;
            itemsetumei.text = null;
        }
    }

    // アイテム数追加.
    public void addItemNum(E_ITEM_TYPE eItemType, int addNum)
    {
        int itemNum = itemkazu[ItemDataBase.GetItemLists()[(int)eItemType - 1]];
        itemNum += addNum;
        itemkazu[ItemDataBase.GetItemLists()[(int)eItemType - 1]] = itemNum;
    }

    // アイテムを使用する.
    public void UseItem(E_ITEM_TYPE eItemType)
    {
        // プレイヤー側でアイテム使用.
        player.UseItem(eItemType);

        // アイテム数を現象させる．
        int itemNum = itemkazu[ItemDataBase.GetItemLists()[(int)eItemType - 1]];
        itemNum--;
        itemkazu[ItemDataBase.GetItemLists()[(int)eItemType - 1]] = itemNum;
    }
}