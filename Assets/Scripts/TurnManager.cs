using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst;

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

        this.isFinish = false;
    }

    [Tooltip("시작 카드 개수를 정합니다")]
    public int startCardCount;

    public bool isStartCardCountMax        //시작 카드 개수가 최대치에 달했는지
    {
        get
        {
            return this.startCardCount >= 6 ? true : false;
        }
    }

    [Header("Properties")]
    public bool isLoading;

    WaitForEndOfFrame delayEndOfFrame = new WaitForEndOfFrame();
    WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action OnAddCard;

    public bool isContinue;
    public bool isTutorialLockCard;
    public bool isTutorialDebuffBar;

    public IEnumerator StartGameCoroutine()
    {
        yield return this.delay01;
        this.isContinue = true;
        this.StartCoroutine(this.StartTurnCoroutine());
    }

    IEnumerator StartTurnCoroutine()
    {
        this.isLoading = true;

        if (this.isTutorialDebuffBar)
        {
            this.isTutorialDebuffBar = false;
            yield return this.StartCoroutine(this.TutorialDebuffBarCoroutine());
        }
        yield return this.delay07;
        if (MapManager.Inst.CurrentSceneName == "전투" || MapManager.Inst.CurrentSceneName == "알 수 없는 공간" || MapManager.Inst.CurrentSceneName == "보스")
        {
            if (Player.Inst.hpbar.sheld > 0)
                Player.Inst.hpbar.Damage(Player.Inst.hpbar.sheld);
            if (EnemyManager.Inst.enemys.Count > 0)
            {
                if (EnemyManager.Inst.enemys[0].hpbar.sheld > 0)
                    EnemyManager.Inst.enemys[0].hpbar.Damage(EnemyManager.Inst.enemys[0].hpbar.sheld);

                if (EnemyManager.Inst.enemys[0].hpbar.turnStartSheld > 0)
                    EnemyManager.Inst.enemys[0].Sheld(EnemyManager.Inst.enemys[0].hpbar.turnStartSheld);
            }
        }
        if (!this.isFinish)
        {
            for (int i = 0; i < this.startCardCount; i++)
            {
                OnAddCard.Invoke();
                yield return this.delay01;
            }
            if (MapManager.Inst.CurrentSceneName == "전투" || MapManager.Inst.CurrentSceneName == "보스" || MapManager.Inst.CurrentSceneName == "알 수 없는 공간")
            {
                SoundManager.Inst.Play(BATTLESOUND.TURN_START);
                GameManager.Inst.Notification("내 턴");
            }
            if (this.isTutorialLockCard)
            {
                CardManager.Inst.LockMyHandCardAll();
                this.isTutorialLockCard = false;
            }
            if (MapManager.Inst.CurrentSceneName == "이벤트") this.StartCoroutine(EventManager.Inst.UpdateBackColorCoroutine());

        }
        yield return this.delay07;
        this.isLoading = false;
    }

    IEnumerator EndTurnCoroutine()
    {
        this.isLoading = true;

        yield return this.delay07;

        for (int i = 0; i < this.startCardCount; i++)
        {
            OnAddCard.Invoke();
            yield return this.delay01;
        }
        GameManager.Inst.Notification("내 턴");
        yield return this.delay07;
        this.isLoading = false;
    }

    IEnumerator TutorialDebuffBarCoroutine()
    {
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[6].Count; i++)
        {
            ArrowManager.Inst.DestoryAllArrow();

            if (i == 0)
                ArrowManager.Inst.CreateArrowObj(StageManager.Inst.debuffTMP.transform.position + new Vector3(0, -1, 0), ArrowCreateDirection.DOWN, StageManager.Inst.debuffTMP.transform);
            else if (i == 1)
                ArrowManager.Inst.CreateArrowObj(StageManager.Inst.debuffTMP.transform.position + new Vector3(-3, 0, 0), ArrowCreateDirection.LEFT, StageManager.Inst.debuffTMP.transform);

            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(6, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }

        ArrowManager.Inst.DestoryAllArrow();

        yield return this.StartCoroutine(TalkWindow.Inst.HideText());

        CardManager.Inst.UnLockMyHandCardAll();

        MapManager.Inst.tutorialIndex++;
    }

    public void EndTurn()
    {
        //EndTurnBtn.SetActive(false);
        //CardManager.Inst.FnishCardAllMyHand();
        this.StartCoroutine(this.EnemyTurnCoroutine());
    }

    IEnumerator EnemyTurnCoroutine()
    {
        while (true)
        {
            if (this.isContinue)
            {
                break;
            }
            yield return this.delayEndOfFrame;
        }
        yield return new WaitForSeconds(1.6f);
        if (EnemyManager.Inst.enemys.Count > 0)
        {
            for (int i = 0; i < EnemyManager.Inst.enemys.Count; i++)
            {
                EnemyManager.Inst.enemys[i].UseTurn();
                yield return new WaitForSeconds(1f);
            }
            for (int i = 0; i < EnemyManager.Inst.enemys.Count; i++)
            {
                EnemyManager.Inst.enemys[i].RandomPatten();
            }
            DebuffManager.Inst.CheckDebuff();
            EnemyManager.Inst.UpdateStateTextAllEnemy();
            this.StartCoroutine(this.StartTurnCoroutine());
        }
    }

    public void AddStartTurnCard()
    {
        if (!this.isStartCardCountMax) this.startCardCount++;
    }

    public IEnumerator ShowReward()    //전투가 끝나거나 이벤트 보상을 얻을때
    {
        RewardManager.Inst.SetFinishBattleReward();
        yield return this.StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine());    //보상 다 받았으면
        MapManager.Inst.LoadMapScene(true);
    }

    public IEnumerator ShowDebuffCoroutine()    //전투에 들어가기 전에 전투 디버프 설정
    {
        RewardManager.Inst.SetRandomBattleDebuff();
        yield return this.StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false));    //보상 다 받았으면
        yield return this.StartCoroutine(RewardManager.Inst.RewardStartBattleCoroutine());
    }

}
