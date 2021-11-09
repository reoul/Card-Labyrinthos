using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EVENT_REWARD_TYPE { CARD, CARD_PIECE, HP, DRAW, QUESTION_CARD, SKILL_BOOK }

public class EventData
{
    public REWARD_KIND reward_kind;
    public int first_reward_probability;
    public EVENT_REWARD_TYPE reward_type1_1;
    public int index1_1;
    public EVENT_REWARD_TYPE reward_type1_2;
    public int index1_2;
    public EVENT_REWARD_TYPE reward_type2;
    public int index2;

    public EventData(REWARD_KIND reward_kind, int first_reward_probability, EVENT_REWARD_TYPE reward_type1_1, int index1_1, EVENT_REWARD_TYPE reward_type1_2, int index1_2, EVENT_REWARD_TYPE reward_type2, int index2)
    {
        this.reward_kind = reward_kind;
        this.first_reward_probability = first_reward_probability;
        this.reward_type1_1 = reward_type1_1;
        this.index1_1 = index1_1;
        this.reward_type1_2 = reward_type1_2;
        this.index1_2 = index1_2;
        this.reward_type2 = reward_type2;
        this.index2 = index2;
    }
}

public class EventManager : MonoBehaviour
{
    public static EventManager Inst = null;

    public List<Event> events;

    public SpriteRenderer[] colorBackSpriteRenderer;
    EventButton[] curEventButtons;

    bool isGetReward = false;

    bool isFinishTutorial = false;

    private void Awake()
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
    }

    public void Choice(EventData eventData)         //조건에 맞는 해당 선택지를 클릭했을때
    {
        if (!isFinishTutorial)
            return;
        isGetReward = true;
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
        RewardManager.Inst.SetTitleText("결과");
        switch (eventData.reward_kind)
        {
            case REWARD_KIND.ONE:
                RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)eventData.reward_type1_1, eventData.index1_1);
                break;
            case REWARD_KIND.TWO:
                RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)eventData.reward_type1_1, eventData.index1_1);
                RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)eventData.reward_type1_2, eventData.index1_2);
                break;
            case REWARD_KIND.RANDOM:
                int rand = Random.Range(0, 100);
                if (rand < eventData.first_reward_probability)
                    RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)eventData.reward_type1_1, eventData.index1_1);
                else
                    RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)eventData.reward_type2, eventData.index2);
                break;
        }
        StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine());
    }

    void FindEvents()
    {
        this.events = new List<Event>();
        Event[] events = GameObject.Find("Event").GetComponentsInChildren<Event>(true);
        for (int i = 0; i < events.Length; i++)
        {
            events[i].gameObject.SetActive(false);
            this.events.Add(events[i]);
        }
        colorBackSpriteRenderer = new SpriteRenderer[3];
        colorBackSpriteRenderer[0] = GameObject.Find("firstColorBack").GetComponent<SpriteRenderer>();
        colorBackSpriteRenderer[1] = GameObject.Find("secondColorBack").GetComponent<SpriteRenderer>();
        colorBackSpriteRenderer[2] = GameObject.Find("thirdColorBack").GetComponent<SpriteRenderer>();
        isGetReward = false;
    }

    public IEnumerator RandomEventCoroutine()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.EVENT);
        FindEvents();
        int rand = Random.Range(0, events.Count);
        events[rand].Init();
        events[rand].gameObject.SetActive(true);
        curEventButtons = new EventButton[3];
        for (int i = 0; i < 3; i++)
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

    public IEnumerator UpdateBackColorCoroutine()      //일정한 시간마다 조건에 맞는 선택지에 초록 불이 들어오고 아닌 선택지는 빨간불이 들어오게 한다
    {
        while (!isGetReward)
        {
            for (int i = 0; i < curEventButtons.Length; i++)
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

    IEnumerator TutorialEventCoroutine()
    {
        yield return new WaitForSeconds(1);
        TalkWindow.Inst.InitFlag();
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[10].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(colorBackSpriteRenderer[0].transform.position + Vector3.up * 2, ArrowCreateDirection.UP);
                ArrowManager.Inst.CreateArrowObj(colorBackSpriteRenderer[1].transform.position + Vector3.up * 2, ArrowCreateDirection.UP);
                ArrowManager.Inst.CreateArrowObj(colorBackSpriteRenderer[2].transform.position + Vector3.up * 2, ArrowCreateDirection.UP);
            }
            Debug.Log(TalkWindow.Inst.talks[10].Count);
            Debug.Log(i);
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(10, i));
            Debug.Log(11111);
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            Debug.Log(22222);
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
            Debug.Log(33333);
            ArrowManager.Inst.DestoryAllArrow();
        }
        Debug.Log("test1");
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishTutorial = true;
    }

    void BackColorGreen(int index)
    {
        colorBackSpriteRenderer[index].color = new Color(60f / 255, 180f / 255, 60f / 255);
    }

    void BackColorRed(int index)
    {
        colorBackSpriteRenderer[index].color = new Color(180f / 255, 60f / 255, 60f / 255);
    }
}
