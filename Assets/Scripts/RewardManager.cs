using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Inst = null;

    public GameObject rewardWindow;
    public GameObject rewardPrefab;

    public List<Reward> rewards;

    public bool isGetAllReward = false;

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

    private void Start()
    {
        activeRewardWindow = false;
        Reward[] rewards = this.GetComponentsInChildren<Reward>(true);
        for (int i = 0; i < rewards.Length; i++)
        {
            this.rewards.Add(rewards[i]);
        }
    }

    private void Update()
    {

    }

    public bool getReward;              //
    public bool activeRewardWindow;     //보상 창이 켜져있는지 확인하는 변수

    public IEnumerator ShowRewardWindowCorutine()
    {
        activeRewardWindow = true;
        rewardWindow.SetActive(true);
        SpriteRenderer windowRenderer = rewardWindow.GetComponent<SpriteRenderer>();
        TMP_Text titleTMP = rewardWindow.transform.GetChild(0).GetComponent<TMP_Text>();

        windowRenderer.color = new Color(255, 255, 255, 0);
        titleTMP.color = new Color(255, 255, 255, 0);
        while (true)
        {
            windowRenderer.color += Color.black * Time.deltaTime * 2;
            titleTMP.color += Color.black * Time.deltaTime * 2;
            if (windowRenderer.color.a > 1)
                break;
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < rewards.Count; i++)
        {
            if (rewards[i].isRewardOn)
                yield return StartCoroutine(rewards[i].FadeCorutine(true));
        }
        StartCoroutine(RewardCorutine());
    }

    public void AddReward(EVENT_REWARD_TYPE reward_type, int index)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            if (!rewards[i].isRewardOn)
            {
                rewards[i].SetReward(reward_type, index);
                rewards[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public IEnumerator RewardCorutine()
    {
        isGetAllReward = false;
        StartCoroutine(CheckGetAllReward());
        while (true)
        {
            if (isGetAllReward)
            {
                isGetAllReward = false;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        activeRewardWindow = false;
        rewardWindow.SetActive(false);
        MapManager.Inst.LoadMapScene(true);
    }

    public IEnumerator CheckGetAllReward()
    {
        while (true)
        {
            int count = 0;
            for (int i = 0; i < rewards.Count; i++)
            {
                if (rewards[i].gameObject.activeInHierarchy)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                isGetAllReward = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GetReward(Reward reward)
    {
        switch (reward.rewardData.reward_type)
        {
            case EVENT_REWARD_TYPE.CARD:
                CardManager.Inst.AddCardDeck(reward.rewardData.index);
                break;
            case EVENT_REWARD_TYPE.CARD_PIECE:
                PlayerManager.Inst.card_piece += reward.rewardData.index;
                break;
            case EVENT_REWARD_TYPE.HP:
                PlayerManager.Inst.hp += reward.rewardData.index;
                break;
            case EVENT_REWARD_TYPE.DRAW:
                break;
        }
        StartCoroutine(reward.FadeCorutine(false));
    }
}
