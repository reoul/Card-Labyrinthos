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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private int _hp;
    [SerializeField] private int _max_hp;
    [SerializeField] private int _card_piece;
    [SerializeField] private int _question_card;
    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = Mathf.Clamp(value, 0, max_hp);
            TopBar.Inst.UpdateText(TOPBAR_TYPE.HP);
        }
    }
    public int card_piece
    {
        get
        {
            return _card_piece;
        }
        set
        {
            _card_piece = Mathf.Clamp(value, 0, 9999); ;
            TopBar.Inst.UpdateText(TOPBAR_TYPE.CARDPIECE);
        }
    }
    public int question_card
    {
        get
        {
            return _question_card;
        }
        set
        {
            _question_card = Mathf.Clamp(value, 0, 99);
            TopBar.Inst.UpdateText(TOPBAR_TYPE.QUESTION);
        }
    }
    public int max_hp { get { return _max_hp; } set { _max_hp = value; } }

    public string hpString { get { return string.Format($"{hp}/{max_hp}"); } }

    public IEnumerator SetupGameCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Player.Inst.hpbar.hp = hp;
        Player.Inst.hpbar.max_hp = max_hp;
        Player.Inst.hpbar.Init();
    }
}
