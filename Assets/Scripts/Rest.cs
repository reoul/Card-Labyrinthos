using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : MonoBehaviour
{
    bool onRestButton = false;   //마우스가 필드 위에 있는지
    bool isTutorial = false;

    private void Start()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.REST);
        if (!MapManager.Inst.isTutorialInRest)
            StartCoroutine(TutorialRestCoroutine());
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
        if (onRestButton && !FadeManager.Inst.isActiveFade)
        {
            StartCoroutine(RestCoroutine());
        }
    }
    IEnumerator RestCoroutine()
    {
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.HP, 20);
        if (!MapManager.Inst.isTutorialInRest)
        {
            TalkWindow.Inst.SetFlagIndex(false);
            TalkWindow.Inst.SetFlagNext(true);
            TalkWindow.Inst.SetSkip(true);
            MapManager.Inst.isTutorialInRest = true;
        }
        while (isTutorial)
        {
            yield return new WaitForEndOfFrame();
        }
        if (!MapManager.Inst.isTutorialInRest)
        {
            yield return new WaitForSeconds(0.5f);
            MapManager.Inst.isTutorialInRest = true;
        }
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false));
        yield return StartCoroutine(RewardManager.Inst.RewardCoroutine());
        TalkWindow.Inst.InitFlag();
        MapManager.Inst.LoadMapScene(true);
    }

    IEnumerator TutorialRestCoroutine()
    {
        isTutorial = true;
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[8].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(this.transform.position + Vector3.right * 2, ArrowCreateDirection.RIGHT);
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(8, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        isTutorial = false;
        Debug.Log("test9");
        yield return StartCoroutine(TalkWindow.Inst.HideText());
    }
}
