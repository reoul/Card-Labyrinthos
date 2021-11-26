using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public class RewardData
{
    public REWARD_TYPE type;
    public EVENT_REWARD_TYPE reward_type;
    public int index;
    public int index2;
}

public class Reward : MonoBehaviour
{
    [SerializeField] TMP_Text resultTMP;
    public RewardData rewardData;
    public DEBUFF_TYPE debuff_type;
    bool onReward;   //마우스가 필드 위에 있는지
    public bool isRewardOn;

    [SerializeField] SpriteRenderer windowRenderer;
    [SerializeField] TMP_Text contentTMP;

    private void Start()
    {
        this.windowRenderer = this.GetComponent<SpriteRenderer>();
        this.contentTMP = this.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void OnMouseEnter()
    {
        this.onReward = true;
    }
    void OnMouseExit()
    {
        this.onReward = false;
    }

    private void OnMouseUp()
    {
        if (this.onReward && !RewardManager.Inst.isChoice)
        {
            RewardManager.Inst.GetReward(this);
        }
    }

    string reward_string
    {
        get
        {
            switch (this.rewardData.reward_type)
            {
                case EVENT_REWARD_TYPE.CARD: return string.Format("{0} 카드를 {1}장 획득합니다", this.rewardData.index + 1, this.rewardData.index2);
                case EVENT_REWARD_TYPE.CARD_PIECE: return string.Format("카드 파편을 {0}개 {1}합니다", Mathf.Abs(this.rewardData.index), this.rewardData.index > 0 ? "획득" : "감소");
                case EVENT_REWARD_TYPE.HP: return string.Format("체력이 {0} {1}합니다", Mathf.Abs(this.rewardData.index), this.rewardData.index > 0 ? "회복" : "감소");
                case EVENT_REWARD_TYPE.DRAW: return string.Format("시작 드로우 개수가 {0}장 증가합니다", Mathf.Abs(this.rewardData.index));
                case EVENT_REWARD_TYPE.QUESTION_CARD: return string.Format("물음표카드를 {0}장 획득합니다", Mathf.Abs(this.rewardData.index));
                case EVENT_REWARD_TYPE.SKILL_BOOK: return "스킬북을 획득합니다";
            }
            return "";
        }
    }
    public void SetReward(REWARD_TYPE type, int reward_type, int index = 0, int index2 = 0)
    {
        this.rewardData.type = type;
        switch (type)
        {
            case REWARD_TYPE.REWARD:
                this.rewardData.reward_type = (EVENT_REWARD_TYPE)reward_type;
                this.rewardData.index = index;
                this.rewardData.index2 = index2;
                this.resultTMP.text = this.reward_string;
                break;
            case REWARD_TYPE.DEBUFF:
                SoundManager.Inst.Play(MAPSOUND.SHOW_DEBUFF_BUTTON);
                this.debuff_type = (DEBUFF_TYPE)reward_type;
                DebuffManager.Inst.debuff_type = this.debuff_type;
                this.resultTMP.text = DebuffManager.Inst.DebuffString;
                break;
        }

        this.isRewardOn = true;
        this.ColorAlpha01(true);
    }
    public IEnumerator FadeCoroutine(bool isOut)
    {
        this.ColorAlpha01(isOut);

        if (isOut)
            SoundManager.Inst.Play(REWARDSOUND.SHOW_REWARD_BUTTON);

        while (true)
        {
            this.windowRenderer.color += Color.black * Time.deltaTime * 2;
            this.contentTMP.color += Color.black * Time.deltaTime * 2;
            if (this.windowRenderer.color.a > 1)
                break;
            yield return new WaitForEndOfFrame();
        }
        if (!isOut)
        {
            this.isRewardOn = false;
            this.gameObject.SetActive(false);
        }
    }

    public void ColorAlpha01(bool isZero)          //컬러의 알파값을 0이나 1로 만들어 준다.
    {
        this.windowRenderer.color = new Color(255, 255, 255, isZero ? 0 : 1);
        this.contentTMP.color = new Color(255, 255, 255, isZero ? 0 : 1);
    }

    public void Init()
    {
        this.onReward = false;
        this.isRewardOn = false;
        this.gameObject.SetActive(false);
    }
}
