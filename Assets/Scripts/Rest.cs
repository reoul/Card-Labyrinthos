using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : MonoBehaviour
{
    bool onRestButton = false;   //마우스가 필드 위에 있는지

    private void Start()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.REST);
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
            StartCoroutine(RestCoroutine());
        }
    }
    IEnumerator RestCoroutine()
    {
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.HP, 20);
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false));
        yield return StartCoroutine(RewardManager.Inst.RewardCoroutine());
        MapManager.Inst.LoadMapScene(true);
    }
}
