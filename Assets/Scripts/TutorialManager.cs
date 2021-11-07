using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] HpBar playerHpBar;
    [SerializeField] TutorialBook tutorialBook;

    bool isGetCard = false;
    bool isGetSkillBook = false;
    bool isStartBattle = false;

    public static TutorialManager Inst = null;
    [SerializeField] Tomb tomb;

    private void Awake()
    {
        Inst = this;
    }


    private void Start()
    {
        StartCoroutine(TutorialCorutine());
    }

    public IEnumerator TutorialCorutine()
    {
        yield return null;
        switch (MapManager.Inst.tutorialIndex)
        {
            case 0:
                StartCoroutine(TutorialCorutine1());
                break;
            case 1:
                StartCoroutine(TutorialCorutine2());
                break;
        }
    }

    public IEnumerator TutorialCorutine1()      //기본 전투 방식 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.TUTORIAL);
        playerHpBar.SetHP(80);
        //
        //yield return new WaitForSeconds(2);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[0].Count; i++)
        {
            if (i == 0)
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, -0.7f, 0), ArrowCreateDirection.RIGHT);
            else if (i == 4)
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, -0.7f, 0), ArrowCreateDirection.RIGHT);
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCorutine(TalkWindow.Inst.index, TalkWindow.Inst.index2));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCorutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCorutine());
            if (i == 3)
            {
                ArrowManager.Inst.DestoryAllArrow();
                //무덤 이미지 보이게 하기
                tomb.gameObject.SetActive(true);
                yield return StartCoroutine(tomb.SetLook());
                ArrowManager.Inst.CreateArrowObj(tomb.transform.position + new Vector3(0, 2f, 0), ArrowCreateDirection.UP);
                yield return StartCoroutine(CheckGetCardCorutine());
                yield return StartCoroutine(CheckGetSkillBookCorutine());
                ArrowManager.Inst.DestoryAllArrow();
            }
        }
        ArrowManager.Inst.DestoryAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        MapManager.Inst.LoadMapScene(true);
        MapManager.Inst.tutorialIndex++;
        yield return null;
    }

    public IEnumerator TutorialCorutine2()      //스킬 사용 설명
    {
        //SoundManager.Inst.Play(BACKGROUNDSOUND.TUTORIAL);
        //
        //yield return new WaitForSeconds(2);
        //Debug.Log("두번째 튜토리얼");
        //yield return new WaitForSeconds(5);
        //카드획득
        //yield return StartCoroutine(BookCorutine());
        //스킬페이지 획득
        //yield return StartCoroutine(GetSkillPageCorutine());
        //전투시작
        SoundManager.Inst.Play(BACKGROUNDSOUND.BATTLE);
        //yield return StartCoroutine(CardManager.Inst.InitCorutine());
        //yield return StartCoroutine(TurnManager.Inst.StartGameCorutine());
        yield return StartCoroutine(StageManager.Inst.CreateStageInTutorial());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[2].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].weaknessTMP.transform.position + new Vector3(0, 1f, 0), ArrowCreateDirection.UP);
                for (int j = 0; j < CardManager.Inst.MyHandCards.Count; j++)
                    CardManager.Inst.MyHandCards[j].isLock = true;
            }
            else if (i == 1 || i == 2)
            {
                ArrowManager.Inst.DestoryAllArrow();
                CardManager.Inst.MyHandCards[0].isLock = false;
                ArrowManager.Inst.CreateArrowObj(CardManager.Inst.MyHandCards[0].transform.position + new Vector3(0, 2, 0), ArrowCreateDirection.UP);
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].hitPos.transform.position + new Vector3(-2, 0, 0), ArrowCreateDirection.LEFT);
            }
            else if (i == 3)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].pattenIndexTMP.transform.position + new Vector3(-0.5f, 1, 0), ArrowCreateDirection.UP);
            }
            else if (i == 4)
            {
                ArrowManager.Inst.DestoryAllArrow();
                CardManager.Inst.MyHandCards[0].isLock = false;
                ArrowManager.Inst.CreateArrowObj(CardManager.Inst.MyHandCards[0].transform.position + new Vector3(0, 2f, 0), ArrowCreateDirection.UP);
                ArrowManager.Inst.CreateArrowObj(Player.Inst.transform.position + new Vector3(1.5f, 1, 0), ArrowCreateDirection.RIGHT);
            }
            else if (i == 5)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(Player.Inst.hpbar.sheldtext.transform.position + new Vector3(-1, 0, 0), ArrowCreateDirection.LEFT);
            }





            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCorutine(TalkWindow.Inst.index, TalkWindow.Inst.index2));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCorutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCorutine());






            //if (i == 3)
            //{
            //    ArrowManager.Inst.DestoryAllArrow();
            //    //무덤 이미지 보이게 하기
            //    tomb.gameObject.SetActive(true);
            //    yield return StartCoroutine(tomb.SetLook());
            //    ArrowManager.Inst.CreateArrowObj(tomb.transform.position + new Vector3(0, 2f, 0), ArrowCreateDirection.UP);
            //    yield return StartCoroutine(CheckGetCardCorutine());
            //    yield return StartCoroutine(CheckGetSkillBookCorutine());
            //    ArrowManager.Inst.DestoryAllArrow();
            //}
        }
        //EnemyManager.Inst.enemys[0].SetFixedWeaknessNum(2);
        //RewardManager.Inst.SetFinishBattleReward();
        //yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCorutine(false));
        //yield return StartCoroutine(RewardManager.Inst.RewardCorutine());
        //MapManager.Inst.LoadMapScene(true);
        //yield return StartCoroutine()
        //MapManager.Inst.LoadMapScene(true);
        yield return null;
    }

    IEnumerator ShowTombCorutine()
    {
        yield return null;
    }

    IEnumerator BookCorutine()
    {
        tutorialBook.gameObject.SetActive(true);
        yield return StartCoroutine(tutorialBook.ShowBook());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(tutorialBook.LoadTextTyping(0));
        yield return StartCoroutine(tutorialBook.icons[0].GetComponent<TutorialBookIcon>().SetLook());
        yield return StartCoroutine(CheckGetCardCorutine());
        yield return StartCoroutine(tutorialBook.LoadTextTyping(1));
        yield return StartCoroutine(tutorialBook.LoadTextTyping(2));
        yield return StartCoroutine(tutorialBook.icons[1].GetComponent<TutorialBookIcon>().SetLook());
        yield return StartCoroutine(CheckGetSkillBookCorutine());
        yield return StartCoroutine(tutorialBook.LoadTextTyping(3));
        yield return StartCoroutine(tutorialBook.icons[2].GetComponent<TutorialBookIcon>().SetLook());
        yield return StartCoroutine(CheckStartBattleCorutine());
    }

    public IEnumerator GetCardCorutine()
    {
        RewardManager.Inst.SetTitleText("무덤");
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 0, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 1, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 2, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 3, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 4, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 5, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.SKILL_BOOK, 1);
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
            case TutorialBookIconType.STARTBATTLE:
                isStartBattle = true;
                break;
        }
    }

    public IEnumerator SetActiveTrueTopBarSkillBook()
    {
        TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).gameObject.SetActive(true);
        isGetSkillBook = true;
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
            {
                tutorialBook.icons[1].GetComponent<TutorialBookIcon>().GetItem();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CheckStartBattleCorutine()
    {
        while (true)
        {
            if (isStartBattle)
            {
                tutorialBook.gameObject.SetActive(false);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
