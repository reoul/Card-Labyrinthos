using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class RewardData
{
    public EVENT_REWARD_TYPE reward_type;
    public int index;
}

public class Reward : MonoBehaviour
{
    [SerializeField] TMP_Text resultTMP;
    public RewardData rewardData;
    bool onReward = false;   //마우스가 필드 위에 있는지
    public bool isRewardOn = false;

    [SerializeField] SpriteRenderer windowRenderer;
    [SerializeField] TMP_Text contentTMP;

    private void Start()
    {
        windowRenderer = this.GetComponent<SpriteRenderer>();
        contentTMP = this.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void OnMouseEnter()
    {
        onReward = true;
    }
    void OnMouseExit()
    {
        onReward = false;
    }

    private void OnMouseUp()
    {
        if (onReward)
        {
            RewardManager.Inst.GetReward(this);
        }
    }

    string reward_string
    {
        get
        {
            switch (rewardData.reward_type)
            {
                case EVENT_REWARD_TYPE.CARD: return string.Format("물음표카드를 {0}장 획득합니다", rewardData.index);
                case EVENT_REWARD_TYPE.CARD_PIECE: return string.Format("카드 파편을 {0}개 {1}합니다", Mathf.Abs(rewardData.index), rewardData.index > 0 ? "획득" : "감소");
                case EVENT_REWARD_TYPE.HP: return string.Format("체력이 {0} {1}합니다", Mathf.Abs(rewardData.index), rewardData.index > 0 ? "증가" : "감소");
                case EVENT_REWARD_TYPE.DRAW: return string.Format("시작 드로우 개수가 {0}장 증가합니다", Mathf.Abs(rewardData.index));
            }
            return "";
        }
    }
    public void SetReward(EVENT_REWARD_TYPE reward_type, int index)
    {
        rewardData.reward_type = reward_type;
        rewardData.index = index;
        resultTMP.text = reward_string;
        isRewardOn = true;
        ColorAlpha01(true);
    }
    public IEnumerator FadeCorutine(bool isOut)
    {
        ColorAlpha01(isOut);

        while (true)
        {
            windowRenderer.color += Color.black * Time.deltaTime * 2;
            contentTMP.color += Color.black * Time.deltaTime * 2;
            if (windowRenderer.color.a > 1)
                break;
            yield return new WaitForEndOfFrame();
        }
        if (!isOut)
        {
            isRewardOn = false;
            this.gameObject.SetActive(false);
        }
    }

    public void ColorAlpha01(bool isZero)          //컬러의 알파값을 0이나 1로 만들어 준다.
    {
        windowRenderer.color = new Color(255, 255, 255, isZero ? 0 : 1);
        contentTMP.color = new Color(255, 255, 255, isZero ? 0 : 1);
    }
}
