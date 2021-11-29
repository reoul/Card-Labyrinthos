using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EVENT_REWARD_TYPE
{
    Card,
    CardPiece,
    Hp,
    Draw,
    QuestionCard,
    SkillBook
}

public class EventData
{
    public readonly REWARD_KIND reward_kind;
    public readonly int first_reward_probability;
    public readonly EVENT_REWARD_TYPE reward_type1_1;
    public readonly int index1_1;
    public readonly EVENT_REWARD_TYPE reward_type1_2;
    public readonly int index1_2;
    public readonly EVENT_REWARD_TYPE reward_type2;
    public readonly int index2;

    public EventData(REWARD_KIND rewardKind, int firstRewardProbability, EVENT_REWARD_TYPE rewardType1_1, int index1_1,
        EVENT_REWARD_TYPE rewardType1_2, int index1_2, EVENT_REWARD_TYPE rewardType2, int index2)
    {
        reward_kind = rewardKind;
        first_reward_probability = firstRewardProbability;
        reward_type1_1 = rewardType1_1;
        this.index1_1 = index1_1;
        reward_type1_2 = rewardType1_2;
        this.index1_2 = index1_2;
        reward_type2 = rewardType2;
        this.index2 = index2;
    }
}

public class EventManager : Singleton<EventManager>
{
    public List<Event> events;

    public SpriteRenderer[] colorBackSpriteRenderer;
    private EventButton[] curEventButtons;

    private bool isGetReward;
    private bool isFinishTutorial;

    private void Awake()
    {
        ExistInstance(this);
    }

    public void Choice(EventData eventData) //조건에 맞는 해당 선택지를 클릭했을때
    {
        if (!isFinishTutorial)
        {
            return;
        }

        isGetReward = true;
        SoundManager.Inst.Play(EVENTSOUND.ChoiceButton);
        RewardManager.Inst.SetTitleText("결과");
        switch (eventData.reward_kind)
        {
            case REWARD_KIND.One:
                RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) eventData.reward_type1_1, eventData.index1_1);
                break;
            case REWARD_KIND.Two:
                RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) eventData.reward_type1_1, eventData.index1_1);
                RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) eventData.reward_type1_2, eventData.index1_2);
                break;
            case REWARD_KIND.Random:
                int rand = Random.Range(0, 100);
                if (rand < eventData.first_reward_probability)
                {
                    RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) eventData.reward_type1_1,
                        eventData.index1_1);
                }
                else
                {
                    RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) eventData.reward_type2, eventData.index2);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventData.reward_kind));
        }

        StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine());
    }

    private void FindEvents()
    {
        this.events = new List<Event>();
        var events = GameObject.Find("Event").GetComponentsInChildren<Event>(true);
        foreach (var _event in events)
        {
            _event.gameObject.SetActive(false);
            this.events.Add(_event);
        }

        colorBackSpriteRenderer = new SpriteRenderer[3];
        colorBackSpriteRenderer[0] = GameObject.Find("firstColorBack").GetComponent<SpriteRenderer>();
        colorBackSpriteRenderer[1] = GameObject.Find("secondColorBack").GetComponent<SpriteRenderer>();
        colorBackSpriteRenderer[2] = GameObject.Find("thirdColorBack").GetComponent<SpriteRenderer>();
        isGetReward = false;
    }

    public IEnumerator RandomEventCoroutine()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.Event);
        FindEvents();

        var rand = Random.Range(0, events.Count);
        events[rand].Init();
        events[rand].gameObject.SetActive(true);

        curEventButtons = new EventButton[3];
        for (var i = 0; i < 3; i++)
        {
            curEventButtons[i] = events[rand].condition_TMP[i].transform.parent.GetComponent<EventButton>();
        }

        if (!MapManager.Inst.isTutorialInEvent)
        {
            MapManager.Inst.isTutorialInEvent = true;
            StartCoroutine(TutorialEventCoroutine());
        }

        yield return null;
    }

    public IEnumerator UpdateBackColorCoroutine() //일정한 시간마다 조건에 맞는 선택지에 초록 불이 들어오고 아닌 선택지는 빨간불이 들어오게 한다
    {
        while (!isGetReward)
        {
            for (var i = 0; i < curEventButtons.Length; i++)
            {
                if (curEventButtons[i].IsAchieve)
                {
                    BackColorGreen(i);
                }
                else
                {
                    BackColorRed(i);
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator TutorialEventCoroutine()
    {
        yield return new WaitForSeconds(1);
        TalkWindow.Inst.InitFlag();
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (var i = 0; i < TalkWindow.Inst.talks[10].Count; i++)
        {
            if (i == 0)
            {
                for (var j = 0; j < 3; j++)
                {
                    ArrowManager.Inst.CreateArrowObj(colorBackSpriteRenderer[j].transform.position + Vector3.up * 2,
                        ArrowCreateDirection.Up);
                }
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(10, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
            ArrowManager.Inst.DestroyAllArrow();
        }

        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishTutorial = true;
    }

    private void BackColorGreen(int index)
    {
        colorBackSpriteRenderer[index].color = new Color(60f / 255, 180f / 255, 60f / 255);
    }

    private void BackColorRed(int index)
    {
        colorBackSpriteRenderer[index].color = new Color(180f / 255, 60f / 255, 60f / 255);
    }
}
