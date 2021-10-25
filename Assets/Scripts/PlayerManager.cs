using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Inst = null;
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
    public int hp { get { return _hp; } set { _hp = value; } }
    public int card_piece { get { return _card_piece; } set { _card_piece = value; } }
    public int max_hp { get { return _max_hp; } set { _max_hp = value; } }

    public string hpString { get { return string.Format($"{hp}/{max_hp}"); } }

    public void SetupGame()
    {
        Player.Inst.hpbar.hp = hp;
        Player.Inst.hpbar.max_hp = max_hp;
    }
}
