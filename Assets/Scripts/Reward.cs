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
    [SerializeField] RewardData rewardData;
    bool onReward = false;   //마우스가 필드 위에 있는지

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
            RewardManager.Inst.GetReward(rewardData);
        }
    }

    string reward_string
    {
        get
        {
            switch (rewardData.reward_type)
            {
                case EVENT_REWARD_TYPE.CARD: return string.Format("랜덤한 숫자의 카드를 {0}장 획득합니다", rewardData.index);
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
    }
}
