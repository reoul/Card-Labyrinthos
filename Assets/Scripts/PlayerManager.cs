using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Inst;
    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    [SerializeField] int _hp;
    [SerializeField] int _max_hp;
    [SerializeField] int _card_piece;
    [SerializeField] int _question_card;
    public int hp
    {
        get
        {
            return this._hp;
        }
        set
        {
            this._hp = Mathf.Clamp(value, 0, this.max_hp);
            TopBar.Inst.UpdateText(TOPBAR_TYPE.HP);
        }
    }
    public int card_piece
    {
        get
        {
            return this._card_piece;
        }
        set
        {
            this._card_piece = Mathf.Clamp(value, 0, 9999); ;
            TopBar.Inst.UpdateText(TOPBAR_TYPE.CARDPIECE);
        }
    }
    public int question_card
    {
        get
        {
            return this._question_card;
        }
        set
        {
            this._question_card = Mathf.Clamp(value, 0, 99);
            TopBar.Inst.UpdateText(TOPBAR_TYPE.QUESTION);
        }
    }
    public int max_hp { get { return this._max_hp; } set { this._max_hp = value; } }

    public string hpString { get { return string.Format($"{this.hp}/{this.max_hp}"); } }

    public IEnumerator SetupGameCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Player.Inst.hpbar.hp = this.hp;
        Player.Inst.hpbar.max_hp = this.max_hp;
        Player.Inst.hpbar.Init();
    }
}
