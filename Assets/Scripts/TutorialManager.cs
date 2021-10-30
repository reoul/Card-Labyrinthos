using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] HpBar playerHpBar;
    private void Start()
    {
        StartCoroutine(TutorialCorutine());
    }

    IEnumerator TutorialCorutine()
    {
        playerHpBar.SetHP(80);
        //
        //카드획득
        //스킬페이지 획득
        //전투시작
        //보상획득
        yield return StartCoroutine(CardManager.Inst.InitCorutine());
        yield return StartCoroutine(TurnManager.Inst.StartGameCorutine());
        yield return StartCoroutine(StageManager.Inst.CreateStageInTutorial());
        //RewardManager.Inst.SetFinishBattleReward();
        //yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCorutine(false));
        //yield return StartCoroutine(RewardManager.Inst.RewardCorutine());
        //MapManager.Inst.LoadMapScene(true);
        //yield return StartCoroutine()
        //MapManager.Inst.LoadMapScene(true);
        yield return null;
    }
}
