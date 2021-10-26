﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HpBar hpbar;         //적 체력바

    public Monster monster;
    public int pattenIndex;          //플레이어에게 줄 데미지
    public int weaknessNum;     //약점카드 숫자
    [SerializeField] PATTERN curPatten;
    int lastPatten;
    int lastWeaknessNum;
    public int force;

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
        force = 0;
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
                Heal();
                break;
        }
    }

    public void Heal()
    {
        hpbar.Heal(pattenIndex);
    }

    public void Attack()
    {
        Player.Inst.Damage(pattenIndex + force);
        pattenIndex = 0;
    }

    public void UpdateStateText()           //텍스트 최신화
    {
        switch (curPatten.pattern_type)
        {
            case PATTERN_TYPE.ATTACK:
                patten_sprite.sprite = StageManager.Inst.attackSprite;
                pattenIndexTMP.text = pattenIndex.ToString() + (force > 0 ? string.Format($" + {force}") : "");
                break;
            case PATTERN_TYPE.HEAL:
                patten_sprite.sprite = StageManager.Inst.healSprite;
                pattenIndexTMP.text = pattenIndex.ToString();
                break;
        }
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
        if (EnemyManager.Inst.enemys.Count == 0)
        {
            StartCoroutine(TurnManager.Inst.ShowReward());
            //CardManager.Inst.FinishBattle();
        }
        Destroy(hpbar.gameObject);
        Destroy(this.gameObject);
    }
}
