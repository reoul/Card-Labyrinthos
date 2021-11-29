using System.Collections;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private void Awake()
    {
        ExistInstance(this);
    }

    [SerializeField] private int _hp;
    [SerializeField] private int _max_hp;
    [SerializeField] private int _card_piece;
    [SerializeField] private int _question_card;

    public int Hp
    {
        get { return _hp; }
        set
        {
            _hp = Mathf.Clamp(value, 0, MAXHp);
            TopBar.Inst.UpdateText(TOPBAR_TYPE.HP);
        }
    }

    public int CardPiece
    {
        get { return _card_piece; }
        set
        {
            _card_piece = Mathf.Clamp(value, 0, 9999);
            ;
            TopBar.Inst.UpdateText(TOPBAR_TYPE.CARDPIECE);
        }
    }

    public int QuestionCard
    {
        get { return _question_card; }
        set
        {
            _question_card = Mathf.Clamp(value, 0, 99);
            TopBar.Inst.UpdateText(TOPBAR_TYPE.QUESTION);
        }
    }

    public int MAXHp
    {
        get { return _max_hp; }
        set { _max_hp = value; }
    }

    public string HpString
    {
        get { return string.Format($"{Hp.ToString()}/{MAXHp.ToString()}"); }
    }

    public IEnumerator SetupGameCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Player.Inst.hpbar.hp = Hp;
        Player.Inst.hpbar.max_hp = MAXHp;
        Player.Inst.hpbar.Init();
    }
}
