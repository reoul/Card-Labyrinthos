using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst = null;

    public bool isFinish;
    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        isFinish = false;
    }

    [Tooltip("시작 카드 개수를 정합니다")]
    public int startCardCount;

    public bool isStartCardCountMax        //시작 카드 개수가 최대치에 달했는지
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
        if (MapManager.Inst.CurrentSceneName == "전투" || MapManager.Inst.CurrentSceneName == "알 수 없는 공간" || MapManager.Inst.CurrentSceneName == "보스")
        {
            if (Player.Inst.hpbar.sheld > 0)
                Player.Inst.hpbar.Damage(Player.Inst.hpbar.sheld);

            if (EnemyManager.Inst.enemys[0].hpbar.sheld > 0)
                EnemyManager.Inst.enemys[0].hpbar.Damage(EnemyManager.Inst.enemys[0].hpbar.sheld);

            if (EnemyManager.Inst.enemys[0].hpbar.turnStartSheld > 0)
                EnemyManager.Inst.enemys[0].hpbar.Sheld(EnemyManager.Inst.enemys[0].hpbar.turnStartSheld);
        }
        if (!isFinish)
        {
            for (int i = 0; i < startCardCount; i++)
            {
                OnAddCard.Invoke();
                yield return delay01;
            }
            if (MapManager.Inst.CurrentSceneName == "전투" || MapManager.Inst.CurrentSceneName == "보스" || MapManager.Inst.CurrentSceneName == "알 수 없는 공간")
            {
                SoundManager.Inst.Play(BATTLESOUND.TURN_START);
                GameManager.Inst.Notification("내 턴");
            }

        }
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
        if (EnemyManager.Inst.enemys.Count > 0)
        {
            DebuffManager.Inst.CheckDebuff();
            EnemyManager.Inst.UpdateStateTextAllEnemy();
            StartCoroutine(StartTurnCorutine());
        }
    }

    public void AddStartTurnCard()
    {
        if (!isStartCardCountMax)
            startCardCount++;
    }

    public IEnumerator ShowReward()    //전투가 끝나거나 이벤트 보상을 얻을때
    {
        RewardManager.Inst.SetFinishBattleReward();
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCorutine());    //보상 다 받았으면
        MapManager.Inst.LoadMapScene(true);
    }

    public IEnumerator ShowDebuffCorutine()    //전투에 들어가기 전에 전투 디버프 설정
    {
        RewardManager.Inst.SetRandomBattleDebuff();
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCorutine(false));    //보상 다 받았으면
        yield return StartCoroutine(RewardManager.Inst.RewardStartBattleCorutine());
    }

}
