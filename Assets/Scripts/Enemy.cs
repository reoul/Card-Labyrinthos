using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HpBar hpbar;         //적 체력바

    public int damage;          //플레이어에게 줄 데미지
    public int weaknessNum;     //약점카드 숫자
    int lastPatten;
    int lastWeaknessNum;

    [SerializeField] TMP_Text damageTMP;        //데미지 텍스트
    [SerializeField] TMP_Text weaknessTMP;      //약점카드 테스트
    public GameObject[] ui;

    private void Start()
    {
        damage = 0;
        weaknessNum = 0;
        lastPatten = 99;
        lastWeaknessNum = 99;
        RandomPatten();
        UpdateStateText();
    }

    public void RandomPatten()      //랜덤 패턴 함수, 턴 끝날때 실행
    {
        do
        {
            weaknessNum = Random.Range(1, 6);
        } while (weaknessNum == lastWeaknessNum);
        lastWeaknessNum = weaknessNum;
        int patten;
        do
        {
            patten = Random.Range(0, 6);
        } while (patten == lastPatten);
        lastPatten = patten;
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

    public void UseTurn()           //적 턴 시작할때 호출됨
    {
        this.GetComponent<Animator>().SetTrigger("Attack");         //공격 애니메이션 실행
    }

    public void Attack()
    {
        if (damage < 20)
            Player.Inst.hpbar.Damage(damage);
        damage = 0;
    }

    public void UpdateStateText()           //텍스트 최신화
    {
        damageTMP.text = damage.ToString();
        weaknessTMP.text = (weaknessNum + 1).ToString();
    }

    public void Damage(int damage)          //적이 공격 당할때 호출
    {
        hpbar.Damage(damage);
    }

    public void Dead()              //적이 죽을때 호출
    {
        for (int i = 0; i < ui.Length; i++)
            ui[i].SetActive(false);
        hpbar.UpdateHp();
        damage = 0;
        this.GetComponent<Animator>().SetTrigger("Dead");
    }

    public void DeadAnimationFinish()       //Dead 애니메이션이 다 끝날때 호출 
    {
        EnemyManager.Inst.enemys.Remove(this);
        TurnManager.Inst.isFinish = true;
        Destroy(hpbar.gameObject);
        Destroy(this.gameObject);
        if (EnemyManager.Inst.enemys.Count == 0)
            GameManager.Inst.Notification("게임 종료");
    }
}
