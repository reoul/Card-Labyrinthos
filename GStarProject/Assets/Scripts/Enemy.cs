using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public HpBar hpbar;

    public int damage;
    public int weaknessNum;

    [SerializeField] TMP_Text damageTMP;
    [SerializeField] TMP_Text weaknessTMP;
    public GameObject[] ui;

    private void Start()
    {
        damage = 0;
        weaknessNum = 0;
        RandomPatten();
        UpdateStateText();
    }

    public void RandomPatten()      //턴 끝날때 실행
    {
        weaknessNum = Random.Range(1, 6);
        int patten = Random.Range(0, 6);
        switch (patten)
        {
            case 0:
                damage = 10;
                break;
            case 1:
                damage = 4;
                break;
            case 2:
                damage = 60;
                break;
            case 3:
                damage = 15;
                break;
            case 4:
                damage = 8;
                break;
            case 5:
                damage = 90;
                break;
            case 6:
                damage = 7;
                break;
            default:
                damage = 5;
                break;
        }
    }

    public void UseTurn()
    {
        this.GetComponent<Animator>().SetTrigger("Attack");
        if(damage < 20)
            Player.Inst.hpbar.Damage(damage);
        damage = 0;
    }

    public void UpdateStateText()
    {
        damageTMP.text = damage.ToString();
        weaknessTMP.text = (weaknessNum+1).ToString();
    }

    public void Damage(int damage)
    {
        hpbar.Damage(damage);
    }

    public void Dead()
    {
        for (int i = 0; i < ui.Length; i++)
            ui[i].SetActive(false);
        hpbar.UpdateHp();
        damage = 0;
        this.GetComponent<Animator>().SetTrigger("Dead");
    }

    public void DeadAnimationFinish()
    {
        EnemyManager.Inst.enemys.Remove(this);
        TurnManager.Inst.isFinish = true;
        Destroy(hpbar.gameObject);
        Destroy(this.gameObject);
        if (EnemyManager.Inst.enemys.Count == 0)
            GameManager.Inst.Notification("게임 종료");
    }
}
