using System.Collections;
using UnityEngine;

public class Rest : MonoBehaviour
{
    private bool onRestButton; //마우스가 필드 위에 있는지
    private bool isTutorial;
    private bool isClick;

    private void Start()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.Rest);
        if (!MapManager.Inst.isTutorialInRest)
        {
            StartCoroutine(TutorialRestCoroutine());
        }
    }

    private void OnMouseEnter()
    {
        onRestButton = true;
    }

    private void OnMouseExit()
    {
        onRestButton = false;
    }

    private void OnMouseUp()
    {
        if (onRestButton && !FadeManager.Inst.isActiveFade && !isTutorial && !isClick)
        {
            StartCoroutine(RestCoroutine());
        }
    }

    private IEnumerator RestCoroutine()
    {
        isClick = true;
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.Hp, 20);
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false));
        yield return StartCoroutine(RewardManager.Inst.RewardCoroutine());
        TalkWindow.Inst.InitFlag();
        MapManager.Inst.LoadMapScene(true);
    }

    private IEnumerator TutorialRestCoroutine()
    {
        isTutorial = true;
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (var i = 0; i < TalkWindow.Inst.talks[8].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(transform.position + Vector3.right * 2, ArrowCreateDirection.Right);
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(8, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }

        ArrowManager.Inst.DestroyAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isTutorial = false;
        MapManager.Inst.isTutorialInRest = true;
    }
}
