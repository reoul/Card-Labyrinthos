using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum REWARD_TYPE { REWARD, DEBUFF }
public enum DEBUFF_TYPE { DEBUFF1, DEBUFF2, DEBUFF3, DEBUFF4, DEBUFF5, DEBUFF6, DEBUFF7, TUTORIAL }

public class RewardManager : MonoBehaviour
{
    public static RewardManager Inst = null;

    public GameObject rewardWindow;
    public TMP_Text titleTMP;

    public List<Reward> rewards;

    public bool isGetAllReward = true;
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
        isGetAllReward = true;
        Reward[] rewards = this.GetComponentsInChildren<Reward>(true);
        for (int i = 0; i < rewards.Length; i++)
        {
            this.rewards.Add(rewards[i]);
        }
    }

    public void Init()
    {
        this.transform.position = new Vector3(0, 0, -1);
    }

    public bool getReward;              //
    public bool activeRewardWindow;     //보상 창이 켜져있는지 확인하는 변수

    public IEnumerator ShowRewardWindowCoroutine(bool isStartRewardCoroutine = true)
    {
        SoundManager.Inst.Play(REWARDSOUND.SHOW_REWARD_WINDOW);
        activeRewardWindow = true;
        rewardWindow.SetActive(true);
        SpriteRenderer windowRenderer = rewardWindow.GetComponent<SpriteRenderer>();
        TMP_Text titleTMP = rewardWindow.transform.GetChild(0).GetComponent<TMP_Text>();

        windowRenderer.color = new Color(255, 255, 255, 0);
        titleTMP.color = new Color(255, 255, 255, 0);

        if (MapManager.Inst.CurrentSceneName != "지도" && MapManager.Inst.CurrentSceneName != "휴식")
            CardManager.Inst.FinishSceneAllMyHand();
        while (true)        //보상창
        {
            windowRenderer.color += Color.black * Time.deltaTime * 2;
            titleTMP.color += Color.black * Time.deltaTime * 2;
            if (windowRenderer.color.a > 1)
                break;
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < rewards.Count; i++)         //보상 선택지
        {
            if (rewards[i].isRewardOn)
                yield return StartCoroutine(rewards[i].FadeCoroutine(true));
        }
        if (isStartRewardCoroutine)
            StartCoroutine(RewardCoroutine());
    }

    public void AddReward(REWARD_TYPE type, int reward_type, int index, int index2 = 0)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            if (!rewards[i].isRewardOn)
            {
                rewards[i].SetReward(type, reward_type, index, index2);
                rewards[i].gameObject.SetActive(true);
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
        if (MapManager.Inst.CurrentSceneName != "휴식" && MapManager.Inst.CurrentSceneName != "지도")
            CardManager.Inst.Init();
        if (isLoadMap)
            MapManager.Inst.LoadMapScene(true);
    }

    public IEnumerator RewardStartBattleCoroutine()
    {
        isGetAllReward = false;
        yield return StartCoroutine(WaitChoiceCoroutine());
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
        SetTitleText("보상");
        int questionCard = Random.Range(0, 2) == 0 ? 1 : 0;
        int cardPiece = Random.Range(20, 40);
        cardPiece += 4 - ((cardPiece % 4) == 0 ? 4 : (cardPiece % 4));
        if (questionCard == 1)
            AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.QUESTION_CARD, questionCard);
        AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD_PIECE, cardPiece);
    }

    public void SetRandomBattleDebuff()
    {
        SoundManager.Inst.Play(MAPSOUND.OPEN_DEBUFFWINDOW);
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
            if (count == 0 && ThrowingObjManager.Inst.moveThrowingReward == 0)
            {
                if (SceneManager.GetActiveScene().name == "Tutorial2")
                {
                    Debug.Log("튜토리얼 보상 다 받음");
                    TalkWindow.Inst.SetFlagIndex(false);
                    TalkWindow.Inst.SetFlagNext(true);
                    TalkWindow.Inst.SetSkip(true);
                    TutorialManager.Inst.isToturialFinish = true;
                    if (MapManager.Inst.tutorialIndex == 1)
                        yield return new WaitForSeconds(2);
                }
                isGetAllReward = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator WaitChoiceCoroutine()
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
                        CardManager.Inst.AddCardDeck(reward.rewardData.index, reward.rewardData.index2);
                        ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.NUM_CARD, reward.transform.position + Vector3.up * 0.5f, TopBar.Inst.GetIcon(TOPBAR_TYPE.BAG).transform.position, null, 1, reward.rewardData.index2, reward.rewardData.index);
                        break;
                    case EVENT_REWARD_TYPE.CARD_PIECE:
                        ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.CARD_PIECE, reward.transform.position + Vector3.up * 0.5f, TopBar.Inst.GetIcon(TOPBAR_TYPE.CARDPIECE).transform.position, null, 1, reward.rewardData.index / 4, 4);
                        break;
                    case EVENT_REWARD_TYPE.HP:
                        if (reward.rewardData.index < 0)
                            SoundManager.Inst.Play(REWARDSOUND.LOSTHEAL);
                        else
                        {
                            SoundManager.Inst.Play(RESTSOUND.HEAL);
                        }
                        PlayerManager.Inst.hp += reward.rewardData.index;
                        break;
                    case EVENT_REWARD_TYPE.DRAW:
                        break;
                    case EVENT_REWARD_TYPE.QUESTION_CARD:
                        ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.QUESTION_CARD, reward.transform.position + Vector3.up * 0.5f, TopBar.Inst.GetIcon(TOPBAR_TYPE.QUESTION).transform.position, null, 1, 1, reward.rewardData.index);
                        break;
                    case EVENT_REWARD_TYPE.SKILL_BOOK:
                        ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.SKILL_BOOK, reward.transform.position + Vector3.up * 0.5f, TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).transform.position, TutorialManager.Inst.SetActiveTrueTopBarSkillBook());
                        break;
                }
                break;
            case REWARD_TYPE.DEBUFF:
                SoundManager.Inst.Play(MAPSOUND.CHOICE_DEBUFF);
                isChoice = true;
                DebuffManager.Inst.debuff_type = reward.debuff_type;
                break;
        }
        StartCoroutine(reward.FadeCoroutine(false));
    }
}
