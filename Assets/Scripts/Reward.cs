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

public class Reward : MouseInteractionObject
{
    [SerializeField] private TMP_Text resultTMP;
    public RewardData rewardData;
    public DEBUFF_TYPE debuff_type;
    public bool isRewardOn;

    [SerializeField] private SpriteRenderer windowRenderer;
    [SerializeField] private TMP_Text contentTMP;

    private void Start()
    {
        windowRenderer = GetComponent<SpriteRenderer>();
        contentTMP = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    private void OnMouseUp()
    {
        if (OnMouse && !RewardManager.Inst.isChoice)
        {
            RewardManager.Inst.GetReward(this);
        }
    }

    private string RewardString
    {
        get
        {
            switch (rewardData.reward_type)
            {
                case EVENT_REWARD_TYPE.Card:
                    return $"{(rewardData.index + 1).ToString()} 카드를 {rewardData.index2.ToString()}장 획득합니다";
                case EVENT_REWARD_TYPE.CardPiece:
                    return
                        $"카드 파편을 {Mathf.Abs(rewardData.index).ToString()}개 {(rewardData.index > 0 ? "획득" : "감소")}합니다";
                case EVENT_REWARD_TYPE.Hp:
                    return $"체력이 {Mathf.Abs(rewardData.index).ToString()} {(rewardData.index > 0 ? "회복" : "감소")}합니다";
                case EVENT_REWARD_TYPE.Draw:
                    return $"시작 드로우 개수가 {Mathf.Abs(rewardData.index).ToString()}장 증가합니다";
                case EVENT_REWARD_TYPE.QuestionCard:
                    return $"물음표카드를 {Mathf.Abs(rewardData.index).ToString()}장 획득합니다";
                case EVENT_REWARD_TYPE.SkillBook:
                    return "스킬북을 획득합니다";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void SetReward(REWARD_TYPE type, int rewardType, int index = 0, int index2 = 0)
    {
        rewardData.type = type;
        switch (type)
        {
            case REWARD_TYPE.Reward:
                rewardData.reward_type = (EVENT_REWARD_TYPE) rewardType;
                rewardData.index = index;
                rewardData.index2 = index2;
                resultTMP.text = RewardString;
                break;
            case REWARD_TYPE.Debuff:
                SoundManager.Inst.Play(MAPSOUND.ShowDebuffButton);
                debuff_type = (DEBUFF_TYPE) rewardType;
                DebuffManager.Inst.debuff_type = debuff_type;
                resultTMP.text = DebuffManager.Inst.DebuffString;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        isRewardOn = true;
        ColorAlpha01(true);
    }

    public IEnumerator FadeCoroutine(bool isOut)
    {
        ColorAlpha01(isOut);

        if (isOut)
        {
            SoundManager.Inst.Play(REWARDSOUND.ShowRewardButton);
        }

        while (true)
        {
            windowRenderer.color += Color.black * Time.deltaTime * 2;
            contentTMP.color += Color.black * Time.deltaTime * 2;
            if (windowRenderer.color.a > 1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        if (!isOut)
        {
            isRewardOn = false;
            gameObject.SetActive(false);
        }
    }

    private void ColorAlpha01(bool isZero) //컬러의 알파값을 0이나 1로 만들어 준다.
    {
        windowRenderer.color = new Color(255, 255, 255, isZero ? 0 : 1);
        contentTMP.color = new Color(255, 255, 255, isZero ? 0 : 1);
    }

    public void Init()
    {
        OnMouse = false;
        isRewardOn = false;
        gameObject.SetActive(false);
    }
}
