using System.Collections;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);
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
            TopBarManager.Inst.UpdateText(TOPBAR_TYPE.Hp);
        }
    }

    public int CardPiece
    {
        get { return _card_piece; }
        set
        {
            _card_piece = Mathf.Clamp(value, 0, 9999);
            ;
            TopBarManager.Inst.UpdateText(TOPBAR_TYPE.CardPiece);
        }
    }

    public int QuestionCard
    {
        get { return _question_card; }
        set
        {
            _question_card = Mathf.Clamp(value, 0, 99);
            TopBarManager.Inst.UpdateText(TOPBAR_TYPE.Question);
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
        Player.Inst.hpbar.Hp = Hp;
        Player.Inst.hpbar.MAXHp = MAXHp;
        Player.Inst.hpbar.Init();
    }
}
