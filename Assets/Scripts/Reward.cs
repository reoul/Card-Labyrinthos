using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Reward : MonoBehaviour
{
    [SerializeField] TMP_Text resultTMP;
    [SerializeField] EVENT_REWARD_TYPE reward_type;
    [SerializeField] int index;
    string reward_string
    {
        get
        {
            switch (reward_type)
            {
                case EVENT_REWARD_TYPE.CARD: return string.Format("랜덤한 숫자의 카드를 {0}장 획득합니다",index);
                case EVENT_REWARD_TYPE.CARD_PIECE: return string.Format("카드 파편을 {0}개 {1}합니다", Mathf.Abs(index), index > 0 ? "획득" : "감소");
                case EVENT_REWARD_TYPE.HP: return string.Format("체력이 {0} {1}합니다", Mathf.Abs(index), index > 0 ? "증가" : "감소");
                case EVENT_REWARD_TYPE.DRAW: return string.Format("시작 드로우 개수가 {0}장 증가합니다", Mathf.Abs(index));
            }
            return "";
        }
    }
    public void SetReward(EVENT_REWARD_TYPE reward_type, int index)
    {
        this.reward_type = reward_type;
        this.index = index;
        resultTMP.text = reward_string;
    }

    public void GetReward()
    {
        switch (reward_type)
        {
            case EVENT_REWARD_TYPE.CARD:
                break;
            case EVENT_REWARD_TYPE.CARD_PIECE:
                break;
            case EVENT_REWARD_TYPE.HP:
                break;
            case EVENT_REWARD_TYPE.DRAW:
                break;
        }
    }
}
