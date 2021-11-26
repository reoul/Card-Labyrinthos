using System.Collections;
using UnityEngine;

public class Rest : MonoBehaviour
{
    bool onRestButton;   //마우스가 필드 위에 있는지
    bool isTutorial;
    bool isClick;

    private void Start()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.REST);
        if (!MapManager.Inst.isTutorialInRest) this.StartCoroutine(this.TutorialRestCoroutine());
    }

    void OnMouseEnter()
    {
        this.onRestButton = true;
    }
    void OnMouseExit()
    {
        this.onRestButton = false;
    }

    private void OnMouseUp()
    {
        if (this.onRestButton && !FadeManager.Inst.isActiveFade && !this.isTutorial && !this.isClick)
        {
            this.StartCoroutine(this.RestCoroutine());
        }
    }
    IEnumerator RestCoroutine()
    {
        this.isClick = true;
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.HP, 20);
        yield return this.StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false));
        yield return this.StartCoroutine(RewardManager.Inst.RewardCoroutine());
        TalkWindow.Inst.InitFlag();
        MapManager.Inst.LoadMapScene(true);
    }

    IEnumerator TutorialRestCoroutine()
    {
        this.isTutorial = true;
        yield return new WaitForSeconds(1);
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[8].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(this.transform.position + Vector3.right * 2, ArrowCreateDirection.RIGHT);
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(8, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        this.isTutorial = false;
        MapManager.Inst.isTutorialInRest = true;
    }
}
