using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst { get; private set; }

    public bool isFinish;
    void Awake()
    {
        Inst = this;
        isFinish = false;
        DontDestroyOnLoad(this.gameObject);
    }

    [Tooltip("시작 카드 개수를 정합니다")] 
    public int startCardCount;

    public bool isStartCardCoutMax        //시작 카드 개수가 최대치에 달했는지
    {
        get
        {
            return startCardCount >= 6 ? true : false;
        }
    }

    [Header("Properties")]
    public bool isLoading;

    WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action OnAddCard;


    public IEnumerator StartGameCorutine()
    {
        yield return delay01;
        StartCoroutine(StartTurnCorutine());
    }

    IEnumerator StartTurnCorutine()
    {
        isLoading = true;

        yield return delay07;
        if (!isFinish)
        {
            for (int i = 0; i < startCardCount; i++)
            {
                OnAddCard.Invoke();
                yield return delay01;
            }
            GameManager.Inst.Notification("내 턴");
        }
        if (Player.Inst.hpbar.sheld > 0)
            Player.Inst.hpbar.Damage(Player.Inst.hpbar.sheld);
        yield return delay07;
        isLoading = false;

    }

    IEnumerator EndTurnCorutine()
    {
        isLoading = true;

        yield return delay07;

        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard.Invoke();
            yield return delay01;
        }
        GameManager.Inst.Notification("내 턴");
        yield return delay07;
        isLoading = false;
    }

    public void EndTurn()
    {
        //EndTurnBtn.SetActive(false);
        //CardManager.Inst.FnishCardAllMyHand();
        StartCoroutine(EnemyTurnCorutine());
    }

    IEnumerator EnemyTurnCorutine()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < EnemyManager.Inst.enemys.Count; i++)
        {
            EnemyManager.Inst.enemys[i].UseTurn();
            yield return new WaitForSeconds(1f);
        }
        for (int i = 0; i < EnemyManager.Inst.enemys.Count; i++)
        {
            EnemyManager.Inst.enemys[i].RandomPatten();
        }
        EnemyManager.Inst.UpdateStateTextAllEnemy();
        StartCoroutine(StartTurnCorutine());
    }

    public void AddStartTurnCard()
    {
        if (!isStartCardCoutMax)
            startCardCount++;
    }

}
