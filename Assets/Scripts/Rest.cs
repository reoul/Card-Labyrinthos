using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : MonoBehaviour
{
    bool onRestButton = false;   //마우스가 필드 위에 있는지

    private void Start()
    {
        SoundManager.Inst.BackGroundPlay(BACKGROUNDSOUND.REST);
    }

    void OnMouseEnter()
    {
        onRestButton = true;
    }
    void OnMouseExit()
    {
        onRestButton = false;
    }

    private void OnMouseUp()
    {
        if (onRestButton)
        {
            StartCoroutine(RestCorutine());
        }
    }
    IEnumerator RestCorutine()
    {
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.HP, 20);
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCorutine(false));
        yield return StartCoroutine(RewardManager.Inst.RewardCorutine());
        MapManager.Inst.LoadMapScene(true);
    }
}
