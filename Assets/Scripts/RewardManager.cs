using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum REWARD_TYPE { REWARD, DEBUFF }
public enum DEBUFF_TYPE { DEBUFF1, DEBUFF2, DEBUFF3, DEBUFF4, DEBUFF5, DEBUFF6, DEBUFF7 }

public class RewardManager : MonoBehaviour
{
    public static RewardManager Inst = null;

    public GameObject rewardWindow;
    public TMP_Text titleTMP;

    public List<Reward> rewards;

    public bool isGetAllReward = false;
    public bool isChoice = false;

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

    public IEnumerator ShowRewardWindowCorutine(bool isStartRewardCorutine = true)
    {
        activeRewardWindow = true;
        rewardWindow.SetActive(true);
        SpriteRenderer windowRenderer = rewardWindow.GetComponent<SpriteRenderer>();
        TMP_Text titleTMP = rewardWindow.transform.GetChild(0).GetComponent<TMP_Text>();

        windowRenderer.color = new Color(255, 255, 255, 0);
        titleTMP.color = new Color(255, 255, 255, 0);

        if (MapManager.Inst.CurrentSceneName != "Map")
            CardManager.Inst.FinishSceneAllMyHand();

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
        if (isStartRewardCorutine)
            StartCoroutine(RewardCorutine());
    }

    public void AddReward(REWARD_TYPE type, int reward_type, int index)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            if (!rewards[i].isRewardOn)
            {
                rewards[i].SetReward(type, reward_type, index);
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
        CardManager.Inst.Init();
        MapManager.Inst.LoadMapScene(true);
    }

    public IEnumerator RewardStartBattleCorutine()
    {
        isGetAllReward = false;
        yield return StartCoroutine(WaitChoiceCorutine());
        isChoice = false;
        activeRewardWindow = false;
        for (int i = 0; i < rewards.Count; i++)
        {
            rewards[i].Init();
        }
        rewardWindow.SetActive(false);
    }

    public void SetFinishBattleReward()
    {
        int questionCard = Random.Range(0, 2) == 0 ? 1 : 0;
        int cardPiece = Random.Range(20, 40);
        if (questionCard == 1)
            AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.QUESTION_CARD, questionCard);
        AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD_PIECE, cardPiece);
    }

    public void SetRandomBattleDebuff()
    {
        SetTitleText("저주");
        int[] choices = new int[3];      //랜덤으로 선택된 3개의 디버프
        int[] Debuffs = new int[7];
        for (int i = 0; i < Debuffs.Length; i++)
        {
            Debuffs[i] = i;
        }
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i] = -1;
        }
        int randomDebuff;
        for (int i = 0; i < choices.Length; i++)
        {
            do
            {
                randomDebuff = Random.Range(0, 7);
            } while (choices[0] == randomDebuff || choices[1] == randomDebuff || choices[2] == randomDebuff);
            choices[i] = randomDebuff;
            AddReward(REWARD_TYPE.DEBUFF, choices[i], 0);
        }
    }

    public void SetTitleText(string title)
    {
        titleTMP.text = title;
    }

    public IEnumerator CheckGetAllReward()
    {
        while (true)
        {
            int count = 0;
            for (int i = 0; i < rewards.Count; i++)
            {
                if (rewards[i].isRewardOn)
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

    public IEnumerator WaitChoiceCorutine()
    {
        while (true)
        {
            if (isChoice)
                break;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GetReward(Reward reward)
    {
        switch (reward.rewardData.type)
        {
            case REWARD_TYPE.REWARD:
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
                    case EVENT_REWARD_TYPE.QUESTION_CARD:
                        PlayerManager.Inst.question_card += reward.rewardData.index;
                        break;
                }
                break;
            case REWARD_TYPE.DEBUFF:
                isChoice = true;
                DebuffManager.Inst.debuff_type = reward.debuff_type;
                break;
        }
        StartCoroutine(reward.FadeCorutine(false));
    }
}
