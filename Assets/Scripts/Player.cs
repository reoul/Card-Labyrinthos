using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        //this.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void Damage(int damage)          //플레이어가 공격 당할때 호출
    {
        GameManager.Inst.CreateHitObj(this.transform.position + new Vector3(0, 1, 0), 0, 1);
        hpbar.Damage(damage);
    }

    public void Dead()
    {
        this.GetComponent<Animator>().SetTrigger("Dead");
        hpbar.UpdateHp();
        TurnManager.Inst.isFinish = true;
    }

    public void DeadAnimationFinish()
    {
        SoundManager.Inst.Play(BATTLESOUND.GAME_FAILD);
        //GameManager.Inst.Notification("게임 종료");
        Destroy(hpbar.gameObject);
        Destroy(this.gameObject);
    }
}
