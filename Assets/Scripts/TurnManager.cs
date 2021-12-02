using System;
using System.Collections;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public bool isFinish;

    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);

        isFinish = false;
    }

    [Tooltip("시작 카드 개수를 정합니다")] public int startCardCount;

    public bool IsStartCardCountMax //시작 카드 개수가 최대치에 달했는지
    {
        get { return startCardCount >= 6; }
    }

    [Header("Properties")] public bool isLoading;

    private readonly WaitForEndOfFrame delayEndOfFrame = new WaitForEndOfFrame();
    private readonly WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action OnAddCard;

    public bool isContinue;
    public bool isTutorialLockCard;
    public bool isTutorialDebuffBar;

    public IEnumerator StartGameCoroutine()
    {
        yield return delay01;
        isContinue = true;
        StartCoroutine(StartTurnCoroutine());
    }

    private IEnumerator StartTurnCoroutine()
    {
        isLoading = true;

        if (isTutorialDebuffBar)
        {
            isTutorialDebuffBar = false;
            yield return StartCoroutine(TutorialDebuffBarCoroutine());
        }

        yield return delay07;
        if (MapManager.CurrentSceneName == "전투" || MapManager.CurrentSceneName == "알 수 없는 공간" ||
            MapManager.CurrentSceneName == "보스")
        {
            if (Player.Inst.hpbar.Shield > 0)
            {
                Player.Inst.hpbar.Damage(Player.Inst.hpbar.Shield);
            }

            if (EnemyManager.Inst.enemys.Count > 0)
            {
                var enemy = EnemyManager.Inst.enemys[0];
                if (enemy.hpbar.Shield > 0)
                {
                    enemy.hpbar.Damage(enemy.hpbar.Shield);
                }

                if (enemy.hpbar.TurnStartShield > 0)
                {
                    enemy.Shield(enemy.hpbar.TurnStartShield);
                }
            }
        }

        if (!isFinish)
        {
            for (var i = 0; i < startCardCount; i++)
            {
                OnAddCard.Invoke();
                yield return delay01;
            }

            if (MapManager.CurrentSceneName == "전투" || MapManager.CurrentSceneName == "보스" ||
                MapManager.CurrentSceneName == "알 수 없는 공간")
            {
                SoundManager.Inst.Play(BATTLESOUND.TurnStart);
                GameManager.Inst.Notification("내 턴");
            }

            if (isTutorialLockCard)
            {
                CardManager.Inst.LockMyHandCardAll();
                isTutorialLockCard = false;
            }

            if (MapManager.CurrentSceneName == "이벤트")
            {
                StartCoroutine(EventManager.Inst.UpdateBackColorCoroutine());
            }
        }

        yield return delay07;
        isLoading = false;
    }

    private IEnumerator TutorialDebuffBarCoroutine()
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (var i = 0; i < TalkWindow.Inst.TalkLists[6].Count; i++)
        {
            ArrowManager.Inst.DestroyAllArrow();

            switch (i)
            {
                case 0:
                    ArrowManager.Inst.CreateArrowObj(
                        StageManager.Inst.debuffTMP.transform.position + new Vector3(0, -1, 0),
                        ArrowCreateDirection.Down, StageManager.Inst.debuffTMP.transform);
                    break;
                case 1:
                    ArrowManager.Inst.CreateArrowObj(
                        StageManager.Inst.debuffTMP.transform.position + new Vector3(-3, 0, 0),
                        ArrowCreateDirection.Left, StageManager.Inst.debuffTMP.transform);
                    break;
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(6, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }

        ArrowManager.Inst.DestroyAllArrow();

        yield return StartCoroutine(TalkWindow.Inst.HideText());

        CardManager.Inst.UnLockMyHandCardAll();

        MapManager.Inst.tutorialIndex++;
    }

    public void EndTurn()
    {
        StartCoroutine(EnemyTurnCoroutine());
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        while (true)
        {
            if (isContinue)
            {
                break;
            }

            yield return delayEndOfFrame;
        }

        yield return new WaitForSeconds(1.6f);

        if (EnemyManager.Inst.enemys.Count > 0)
        {
            foreach (var enemy in EnemyManager.Inst.enemys)
            {
                enemy.UseTurn();
                yield return new WaitForSeconds(1f);
            }

            foreach (var enemy in EnemyManager.Inst.enemys)
            {
                enemy.RandomPatten();
            }

            DebuffManager.Inst.CheckDebuff();
            EnemyManager.Inst.UpdateStateTextAllEnemy();
            StartCoroutine(StartTurnCoroutine());
        }
    }

    public void AddStartTurnCard()
    {
        if (!IsStartCardCountMax)
        {
            startCardCount++;
        }
    }

    /// <summary>
    /// 전투가 끝나거나 이벤트 보상을 얻을 때 보상을 보여줌.
    /// </summary>
    public IEnumerator ShowReward()
    {
        RewardManager.Inst.SetFinishBattleReward();
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine()); //보상 다 받았으면
        MapManager.Inst.LoadMapScene(true);
    }

    /// <summary>
    /// 전투에 들어가기 전에 전투 디버프 설정
    /// </summary>
    public IEnumerator ShowDebuffCoroutine()
    {
        RewardManager.Inst.SetRandomBattleDebuff();
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false)); //보상 다 받았으면
        yield return StartCoroutine(RewardManager.Inst.RewardStartBattleCoroutine());
    }
}
