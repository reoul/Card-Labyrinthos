using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] HpBar playerHpBar;
    [SerializeField] TutorialBook tutorialBook;

    bool isGetCard = false;
    bool isGetSkillBook = false;

    public static TutorialManager Inst = null;

    private void Awake()
    {
        Inst = this;
    }


    private void Start()
    {
        StartCoroutine(TutorialCorutine());
    }

    IEnumerator TutorialCorutine()
    {
        playerHpBar.SetHP(80);
        //
        yield return new WaitForSeconds(1);
        //카드획득
        yield return StartCoroutine(BookCorutine());
        //스킬페이지 획득
        yield return StartCoroutine(GetSkillPageCorutine());
        //전투시작
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

    IEnumerator BookCorutine()
    {
        tutorialBook.gameObject.SetActive(true);
        yield return StartCoroutine(tutorialBook.LoadTextTyping(0));
        yield return StartCoroutine(tutorialBook.icons[0].GetComponent<TutorialBookIcon>().SetLook());
        yield return StartCoroutine(CheckGetCardCorutine());
        yield return StartCoroutine(tutorialBook.LoadTextTyping(1));
        yield return StartCoroutine(tutorialBook.LoadTextTyping(2));
        yield return StartCoroutine(tutorialBook.icons[1].GetComponent<TutorialBookIcon>().SetLook());
        yield return StartCoroutine(CheckGetSkillBookCorutine());
        yield return StartCoroutine(tutorialBook.LoadTextTyping(3));
    }

    public IEnumerator GetCardCorutine()
    {
        RewardManager.Inst.SetTitleText("카드 뭉치");
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 0, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 1, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 2, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 3, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 4, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 5, 1);
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCorutine(false));
        yield return StartCoroutine(RewardManager.Inst.RewardCorutine(false));
        RewardManager.Inst.transform.GetChild(0).gameObject.SetActive(false);
        isGetCard = true;
    }

    IEnumerator GetSkillPageCorutine()
    {
        yield return null;
    }

    public void Click(TutorialBookIcon icon)
    {
        switch (icon.type)
        {
            case TutorialBookIconType.CARDS:
                StartCoroutine(GetCardCorutine());
                break;
            case TutorialBookIconType.SKILL:
                ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.SKILL_BOOK, icon.transform.position, TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).transform.position, SetActiveTrueTopBarSkillBook());
                break;
        }
    }

    IEnumerator SetActiveTrueTopBarSkillBook()
    {
        TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).gameObject.SetActive(true);
        yield return null;
    }

    IEnumerator CheckGetCardCorutine()
    {
        while (true)
        {
            if (isGetCard)
            {
                tutorialBook.icons[0].GetComponent<TutorialBookIcon>().GetItem();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CheckGetSkillBookCorutine()
    {
        while (true)
        {
            if (isGetSkillBook)
                break;
            yield return new WaitForEndOfFrame();
        }
    }
}
