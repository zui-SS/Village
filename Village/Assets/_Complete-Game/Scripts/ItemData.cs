using UnityEngine;


[CreateAssetMenu(menuName = "Data/Create ItemData")]
public class ItemData : ScriptableObject
{
    public enum itemtype
    {
        Sword, rod, clothes, armor, recovery, nomal, important

    }

    [SerializeField]
    private string Itemname; //�A�C�e���̖��O
    [SerializeField]
    private itemtype Itemtype; //�A�C�e���̃^�C�v
    [SerializeField]
    private Sprite Itemicon; //�A�C�e���̃A�C�R��
    [SerializeField]
    private string Itemexplanation; //�A�C�e���̐���
    [SerializeField]
    private int Itemlimit; //�A�C�e���̎��Ă�ő��


    public string GetItemname()
    {
        return Itemname;
    }

    public itemtype GetItemtype()
    {
        return Itemtype;
    }

    public Sprite GetItemicon()
    {
        return Itemicon;
    }

    public string GetItemexplanation()
    {
        return Itemexplanation;
    }


    public int GetItemlimit()
    {
        return Itemlimit;
    }

}