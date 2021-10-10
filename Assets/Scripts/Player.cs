using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Inst;

    public HpBar hpbar;

    void Awake()
    {
        Inst = this;
    }

    public void Attack()
    {
        this.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void Dead()
    {
        this.GetComponent<Animator>().SetTrigger("Dead");
        hpbar.UpdateHp();
        TurnManager.Inst.isFinish = true;
    }
    public void DeadAnimationFinish()
    {
        GameManager.Inst.Notification("게임 종료");
        Destroy(hpbar.gameObject);
        Destroy(this.gameObject);
    }
}
