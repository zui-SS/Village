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


    //������e�A�C�e���X���b�g��icon���w��(�X���b�g�̔ԍ�����)
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

    //�g�O���O���[�v�ł���inventory���w��B
    [SerializeField] ToggleGroup togglegroup;

    //�A�C�e�������\����
    //��������TextMeshPro���w��B
    [SerializeField] TextMeshProUGUI itemname;

    [SerializeField] TextMeshProUGUI itemsetumei;


    //�@�A�C�e�����Ǘ�
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();

    //�������Ǘ�
    List<ItemData> MotimonoList = new List<ItemData>();


    //�A�C�R���Ǘ��̔z��
    Image[] Icons = new Image[10];


    // Start is called before the first frame update
    void Start()
    {
        //�������A�C�e������

        for (int i = 0; i < ItemDataBase.GetItemLists().Count; i++)
        {
            //�@�A�C�e������S��0��
            itemkazu.Add(ItemDataBase.GetItemLists()[i], 0);
        }


        //�����Ă��鏉���A�C�e���ݒ�B

        //�|�[�V�����̐���2�ɂ���B
        itemkazu[ItemDataBase.GetItemLists()[1]] = 2;
        //�����O�\�[�h�̐���1�ɂ���B
        itemkazu[ItemDataBase.GetItemLists()[0]] = 1;

        //�������X�V�������Ăяo���B
        Motimonokoushin();


    }


    //�ǂ�����ł��A�N�Z�X�\�B�Ԃ�l�Ȃ��B
    public void Motimonokoushin()
    {

        //�������X�V����

        //���������X�g�̃N���A
        MotimonoList.Clear();

        //�����Ă������1�ȏ�̃A�C�e�������������X�g�ɒǉ�����B
        for (int i = 0; i < itemkazu.Count; i++)
        {
            var e = itemkazu[ItemDataBase.GetItemLists()[i]];

            if (e > 0)
            {
                MotimonoList.Add(ItemDataBase.GetItemLists()[i]);
            }
        }

        //�A�C�e���X���b�g�̃A�C�R��image��GetComponent���ăA�C�R���z��ɑ���B
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

        //�e�A�C�R���̉摜��null�ɂ���(��������S�ċ�X���b�g�Ƃ���)�B

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

        //�e�A�C�R���̃J���[�ݒ�B�ȉ��̐��l���ƊD�F�̂悤�ȐF�ƂȂ�(��X���b�g�\���p)

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

        //���������X�g�̗v�f�������J��Ԃ��B
        for (int i = 0; i < MotimonoList.Count; i++)
        {
            //���������X�g��i�Ԗڂ�f�ɑ���B
            var f = MotimonoList[i];
            //f�̃A�C�R����z��ɑ���B
            Icons[i].sprite = f.GetItemicon();
            //�z��̃A�C�R���̃J���[�𔒐F�ɂ���(�A�C�R�������₷�����邽��)�B
            Icons[i].color = new Color(1, 1, 1, 1);
        }
    }


    public void slotkoushin()
    {
        //�X���b�g�X�V����
        //�I������Ă���g�O��(�܂�I������Ă���A�C�e���X���b�g)�����B
        Toggle tgl = togglegroup.ActiveToggles().FirstOrDefault();

        //�I������Ă���g�O���̃Q�[���I�u�W�F�N�g�������B
        string x = tgl.name;

        //Parse�ɂ�蕶����̐�����int�^��float�^�ɂł���B�����int�^�ɂ���y�ɑ���B
        int y = int.Parse(x);


        //���������X�g�̗v�f����y�ȏォ�ǂ������m�F����B
        if (MotimonoList.Count >= y)
        {
            //�I������Ă���g�O���̓A�C�R���\������Ă���B

            //���������X�g��y-1��(���X�g���n�܂�̂�0�Ԃ���ł��邽��)�̖��O�ƌ���Z�AK�ɑ���B
            string z = MotimonoList[y - 1].GetItemname();

            int k = itemkazu[MotimonoList[y - 1]];


            //Z��K�����킹�ăA�C�e�����̕�������쐬��text�ɏo���B
            itemname.text = ($"{z}�~{k}");

            //���������X�g�̃A�C�e���̐�����j�ɑ�����邵text�ɏo���B
            string j = MotimonoList[y - 1].GetItemexplanation();
            itemsetumei.text = j;
        }
        else
        {
            //�����ɓ��Ă͂܂�Ȃ������ꍇ�I�����ꂽ�A�C�e���X���b�g���󂾂����Ƃ������ƁB�����text�ɂ�null��������B
            itemname.text = null;
            itemsetumei.text = null;
        }
    }

    // �A�C�e�����ǉ�.
    public void addItemNum(E_ITEM_TYPE eItemType, int addNum)
    {
        int itemNum = itemkazu[ItemDataBase.GetItemLists()[(int)eItemType - 1]];
        itemNum += addNum;
        itemkazu[ItemDataBase.GetItemLists()[(int)eItemType - 1]] = itemNum;
    }

    // �A�C�e�����g�p����.
    public void UseItem(E_ITEM_TYPE eItemType)
    {
        // �v���C���[���ŃA�C�e���g�p.
        player.UseItem(eItemType);

        // �A�C�e���������ۂ�����D
        int itemNum = itemkazu[ItemDataBase.GetItemLists()[(int)eItemType - 1]];
        itemNum--;
        itemkazu[ItemDataBase.GetItemLists()[(int)eItemType - 1]] = itemNum;
    }
}