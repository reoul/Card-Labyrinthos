using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HpBar hpbar;         //적 체력바

    public Monster monster;
    public int pattenIndex;          //플레이어에게 줄 데미지
    public int weaknessNum;     //약점카드 숫자
    PATTERN curPatten;
    int lastPatten;
    int lastWeaknessNum;

    [SerializeField] SpriteRenderer patten_sprite;
    [SerializeField] TMP_Text pattenIndexTMP;        //데미지 텍스트
    [SerializeField] TMP_Text weaknessTMP;      //약점카드 테스트
    public GameObject[] ui;

    private void Start()
    {
        pattenIndex = 0;
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
            patten = Random.Range(0, 4);
        } while (patten == lastPatten);
        lastPatten = patten;
        switch (patten)
        {
            case 0:
                curPatten = monster.pattern_1;
                break;
            case 1:
                curPatten = monster.pattern_2;
                break;
            case 2:
                curPatten = monster.pattern_3;
                break;
            case 3:
                curPatten = monster.pattern_4;
                break;
        }
        pattenIndex = curPatten.index;
    }


    public void UseTurn()           //적 턴 시작할때 호출됨
    {
        switch (curPatten.pattern_type)
        {
            case PATTERN_TYPE.ATTACK:
                this.GetComponent<Animator>().SetTrigger("Attack");         //공격 애니메이션 실행
                break;
            case PATTERN_TYPE.HEAL:
                break;
        }
    }

    public void Heal()
    {

    }

    public void Attack()
    {
        if (pattenIndex < 20)
            Player.Inst.hpbar.Damage(pattenIndex);
        pattenIndex = 0;
    }

    public void UpdateStateText()           //텍스트 최신화
    {
        pattenIndexTMP.text = pattenIndex.ToString();
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
        pattenIndex = 0;
        this.GetComponent<Animator>().SetTrigger("Dead");
    }

    public void DeadAnimationFinish()       //Dead 애니메이션이 다 끝날때 호출 
    {
        EnemyManager.Inst.enemys.Remove(this);
        TurnManager.Inst.isFinish = true;
        Destroy(hpbar.gameObject);
        Destroy(this.gameObject);
        if (EnemyManager.Inst.enemys.Count == 0)
        {
            GameManager.Inst.Notification("전투 승리");
            CardManager.Inst.FinishBattle();
        }
    }
}
