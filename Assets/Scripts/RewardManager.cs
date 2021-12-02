using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum REWARD_TYPE
{
    Reward,
    Debuff
}

public enum DEBUFF_TYPE
{
    Debuff1,
    Debuff2,
    Debuff3,
    Debuff4,
    Debuff5,
    Debuff6,
    Debuff7,
    Tutorial
}

public class RewardManager : Singleton<RewardManager>
{
    public GameObject rewardWindow;
    public TMP_Text titleTMP;

    public List<Reward> rewards;

    public bool isGetAllReward = true;
    public bool isChoice;

    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);
    }

    private void Start()
    {
        activeRewardWindow = false;
        isGetAllReward = true;
        var rewards = GetComponentsInChildren<Reward>(true);
        foreach (var reward in rewards)
        {
            this.rewards.Add(reward);
        }
    }

    public void Init()
    {
        transform.position = new Vector3(0, 0, -1);
    }

    public bool activeRewardWindow; //보상 창이 켜져있는지 확인하는 변수

    public IEnumerator ShowRewardWindowCoroutine(bool isStartRewardCoroutine = true)
    {
        SoundManager.Inst.Play(REWARDSOUND.ShowRewardWindow);
        activeRewardWindow = true;
        rewardWindow.SetActive(true);
        var windowRenderer = rewardWindow.GetComponent<SpriteRenderer>();
        var titleTMP = rewardWindow.transform.GetChild(0).GetComponent<TMP_Text>();

        windowRenderer.color = new Color(255, 255, 255, 0);
        titleTMP.color = new Color(255, 255, 255, 0);

        if (MapManager.CurrentSceneName != "지도" && MapManager.CurrentSceneName != "휴식")
        {
            CardManager.Inst.FinishSceneAllMyHand();
        }

        Tween fade1 = windowRenderer.DOFade(1, 0.5f);
        Tween fade2 = titleTMP.DOFade(1, 0.5f);

        //보상창 페이드
        yield return fade1.WaitForCompletion();
        yield return fade2.WaitForCompletion();

        foreach (var reward in rewards)
        {
            if (reward.isRewardOn)
            {
                yield return StartCoroutine(reward.FadeCoroutine(true));
            }
        }

        if (isStartRewardCoroutine)
        {
            StartCoroutine(RewardCoroutine());
        }
    }

    public void AddReward(REWARD_TYPE type, int rewardType, int index, int index2 = 0)
    {
        foreach (var reward in rewards)
        {
            if (!reward.isRewardOn)
            {
                reward.SetReward(type, rewardType, index, index2);
                reward.gameObject.SetActive(true);
                break;
            }
        }
    }

    public IEnumerator RewardCoroutine(bool isLoadMap = true)
    {
        isGetAllReward = false;
        StartCoroutine(CheckGetAllReward());
        while (true)
        {
            if (isGetAllReward)
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        activeRewardWindow = false;
        rewardWindow.SetActive(false);
        if (MapManager.CurrentSceneName != "휴식" && MapManager.CurrentSceneName != "지도")
        {
            CardManager.Inst.Init();
        }

        if (isLoadMap)
        {
            MapManager.Inst.LoadMapScene(true);
        }
    }

    /// <summary>
    /// 저주를 골랐는지 확인하는 코루틴, 저주를 선택하기 전까지는 종료되지 않는다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RewardStartBattleCoroutine()
    {
        isGetAllReward = false;
        yield return StartCoroutine(WaitChoiceCoroutine());
        isChoice = false;
        activeRewardWindow = false;
        foreach (var reward in rewards)
        {
            reward.Init();
        }

        rewardWindow.SetActive(false);
    }

    /// <summary>
    /// 전투가 끝나고 보상을 설정한다.
    /// </summary>
    public void SetFinishBattleReward()
    {
        SetTitleText("보상");
        var questionCard = Random.Range(1, 3);
        var cardPiece = Random.Range(100, 120);

        cardPiece += 10 - ((cardPiece % 10) == 0 ? 10 : (cardPiece % 10));

        AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.QuestionCard, questionCard);
        AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.CardPiece, cardPiece);
    }

    /// <summary>
    /// 전투 필드 선택 시 저주를 설정한다.
    /// </summary>
    public void SetRandomBattleDebuff()
    {
        SoundManager.Inst.Play(MAPSOUND.OpenDebuffWindow);
        SetTitleText("저주");
        var choices = new int[3]; //랜덤으로 선택된 3개의 디버프
        var debuffs = new int[7];
        for (int i = 0; i < debuffs.Length; i++)
        {
            debuffs[i] = i;
        }

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i] = -1;
        }

        for (int i = 0; i < choices.Length; i++)
        {
            int randomDebuff;
            do
            {
                randomDebuff = Random.Range(0, 7);
            } while (choices[0] == randomDebuff || choices[1] == randomDebuff || choices[2] == randomDebuff);

            choices[i] = randomDebuff;
            AddReward(REWARD_TYPE.Debuff, choices[i], 0);
        }
    }

    /// <summary>
    /// 보상창 제목을 설정한다.
    /// </summary>
    /// <param name="title">제목</param>
    public void SetTitleText(string title)
    {
        titleTMP.text = title;
    }

    /// <summary>
    /// 보상을 모두 받을때까지 코루틴이 종료되지 않는다.
    /// </summary>
    private IEnumerator CheckGetAllReward()
    {
        while (true)
        {
            var count = 0;
            foreach (var reward in rewards)
            {
                if (reward.isRewardOn)
                {
                    count++;
                }
            }

            if (count == 0 && ThrowingObjManager.Inst.MoveThrowingReward == 0)
            {
                if (SceneManager.GetActiveScene().name == "Tutorial2")
                {
                    TalkWindow.Inst.SetFlagIndex(false);
                    TalkWindow.Inst.SetFlagNext(true);
                    TalkWindow.Inst.SetSkip(true);
                    if (MapManager.Inst.tutorialIndex == 1)
                    {
                        yield return new WaitForSeconds(2);
                    }
                }

                isGetAllReward = true;
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 보상 중 하나를 선택했는지 확인하는 코루틴, 하나를 선택하기 전까지는 종료되지 않는다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitChoiceCoroutine()
    {
        while (true)
        {
            if (isChoice)
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 보상을 얻는다.
    /// </summary>
    /// <param name="reward">얻을 보상</param>
    public void GetReward(Reward reward)
    {
        switch (reward.rewardData.type)
        {
            case REWARD_TYPE.Reward:
                switch (reward.rewardData.reward_type)
                {
                    case EVENT_REWARD_TYPE.Card:
                        CardManager.Inst.AddCardDeck(reward.rewardData.index, reward.rewardData.index2);
                        ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.NumCard,
                            reward.transform.position + Vector3.up * 0.5f,
                            TopBarManager.Inst.GetIcon(TOPBAR_TYPE.Bag).transform.position, null, 1, reward.rewardData.index2,
                            reward.rewardData.index);
                        break;
                    case EVENT_REWARD_TYPE.CardPiece:
                        if (reward.rewardData.index > 0)
                        {
                            ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.CardPiece,
                                reward.transform.position + Vector3.up * 0.5f,
                                TopBarManager.Inst.GetIcon(TOPBAR_TYPE.CardPiece).transform.position, null, 1,
                                reward.rewardData.index / 10, 10);
                        }
                        else
                        {
                            PlayerManager.Inst.CardPiece += reward.rewardData.index;
                        }

                        break;
                    case EVENT_REWARD_TYPE.Hp:
                        if (reward.rewardData.index < 0)
                        {
                            SoundManager.Inst.Play(REWARDSOUND.LostHeal);
                        }
                        else
                        {
                            SoundManager.Inst.Play(RESTSOUND.Heal);
                        }

                        PlayerManager.Inst.Hp += reward.rewardData.index;
                        break;
                    case EVENT_REWARD_TYPE.Draw:
                        break;
                    case EVENT_REWARD_TYPE.QuestionCard:
                        ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.QuestionCard,
                            reward.transform.position + Vector3.up * 0.5f,
                            TopBarManager.Inst.GetIcon(TOPBAR_TYPE.Question).transform.position, null, 1,
                            reward.rewardData.index, 1);
                        break;
                    case EVENT_REWARD_TYPE.SkillBook:
                        ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.SkillBook,
                            reward.transform.position + Vector3.up * 0.5f,
                            TopBarManager.Inst.GetIcon(TOPBAR_TYPE.Skill).transform.position,
                            TutorialManager.Inst.SetActiveTrueTopBarSkillBook());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case REWARD_TYPE.Debuff:
                SoundManager.Inst.Play(MAPSOUND.ChoiceDebuff);
                isChoice = true;
                DebuffManager.Inst.debuff_type = reward.debuff_type;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        StartCoroutine(reward.FadeCoroutine(false));
    }
}
