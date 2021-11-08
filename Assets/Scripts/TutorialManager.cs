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

    public bool isWin;

    public bool isToturialFinish = false;
    public bool isToturialOpenSkill = false;

    private void Awake()
    {
        Inst = this;
    }


    private void Start()
    {
        StartCoroutine(TutorialCoroutine());
    }

    public IEnumerator TutorialCoroutine()
    {
        yield return null;
        switch (MapManager.Inst.tutorialIndex)
        {
            case 0:
                StartCoroutine(TutorialStoryCoroutine());
                break;
            case 1:
                StartCoroutine(TutorialBattleCoroutine());
                break;
            case 2:
                StartCoroutine(TutorialSkillCoroutine());
                break;
        }
    }

    public IEnumerator TutorialStoryCoroutine()      //기본 스토리 설명
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
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(TalkWindow.Inst.index, TalkWindow.Inst.index2));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
            if (i == 3)
            {
                ArrowManager.Inst.DestoryAllArrow();
                //무덤 이미지 보이게 하기
                tomb.gameObject.SetActive(true);
                yield return StartCoroutine(tomb.SetLook());
                ArrowManager.Inst.CreateArrowObj(tomb.transform.position + new Vector3(1.5f, 0, 0), ArrowCreateDirection.RIGHT);
                yield return StartCoroutine(CheckGetCardCoroutine());
                yield return StartCoroutine(CheckGetSkillBookCoroutine());
                ArrowManager.Inst.DestoryAllArrow();
            }
        }
        ArrowManager.Inst.DestoryAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        MapManager.Inst.LoadMapScene(true);
        MapManager.Inst.tutorialIndex++;
        yield return null;
    }

    public IEnumerator TutorialBattleCoroutine()      //기본 전투 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.BATTLE);

        yield return StartCoroutine(StageManager.Inst.CreateStageInTutorial());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());

        for (int i = 0; i < TalkWindow.Inst.talks[2].Count; i++)
        {
            if (i == 0)     //앞에 몬스터 머리 위에 있는 3이라는 숫자가 보이나? 그 숫자가 적의 약점 숫자라네.
            {
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].weaknessTMP.transform.position + new Vector3(0, 1f, 0), ArrowCreateDirection.UP);
                for (int j = 0; j < CardManager.Inst.MyHandCards.Count; j++)
                    CardManager.Inst.MyHandCards[j].isLock = true;
            }
            else if (i == 1 || i == 2)  //손에 들고 있는 3 카드로 한번 공격해보게. 그 숫자 그대로 데미지가 들어갈 거야., 이번엔 6 카드로 한번 공격해봐. 약점 숫자랑 다르다면 데미지가 1밖에 안 들어갈 거네.
            {
                ArrowManager.Inst.DestoryAllArrow();
                CardManager.Inst.MyHandCards[0].isLock = false;
                ArrowManager.Inst.CreateArrowObj(CardManager.Inst.MyHandCards[0].transform.position + new Vector3(0, 2, 0), ArrowCreateDirection.UP, CardManager.Inst.MyHandCards[0].transform);
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].hitPos.transform.position + new Vector3(-2, 0, 0), ArrowCreateDirection.LEFT);
                TalkWindow.Inst.SetFlagIndex(true);
                CardManager.Inst.isTutorial = true;
            }
            else if (i == 3)    //약점 숫자 뒤에 아이콘은 적의 패턴이라네. 검일 땐 공격, 십자가일 땐 회복이지.
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].pattenIndexTMP.transform.position + new Vector3(-0.5f, 1, 0), ArrowCreateDirection.UP);
            }
            else if (i == 4)    //나머지 카드를 자네에게 써보게. 그렇다면 해당 숫자만큼의 실드가 생길 거야
            {
                ArrowManager.Inst.DestoryAllArrow();
                CardManager.Inst.MyHandCards[0].isLock = false;
                ArrowManager.Inst.CreateArrowObj(CardManager.Inst.MyHandCards[0].transform.position + new Vector3(0, 2f, 0), ArrowCreateDirection.UP, CardManager.Inst.MyHandCards[0].transform);
                ArrowManager.Inst.CreateArrowObj(Player.Inst.transform.position + new Vector3(1.5f, 1, 0), ArrowCreateDirection.RIGHT);
                TalkWindow.Inst.SetFlagIndex(true);
                TurnManager.Inst.isContinue = false;
            }
            else if (i == 5)    //실드는 적의 공격을 실드 숫자만큼 방어해주지.
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(Player.Inst.hpbar.sheldtext.transform.position + new Vector3(-1, 0, 0), ArrowCreateDirection.LEFT);
                CardManager.Inst.isTutorial = false;
            }
            else if (i == 6)    //그리고 손에 들고 있는 카드를 다 사용하면 몬스터의 턴으로 넘어가게 된다네. 계속 싸워서 이겨보게나.
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, -0.7f, 0), ArrowCreateDirection.RIGHT);
            }
            else if (i == 7)    //이겼을 때는 일정 확률로 물음표 카드와 카드 파편을 주지. 보유 개수는 화살표가 가리키는 곳에서 확인할 수 있다네.
            {
                ArrowManager.Inst.CreateArrowObj(TopBar.Inst.GetIcon(TOPBAR_TYPE.CARDPIECE).transform.position + new Vector3(0, -1, 0), ArrowCreateDirection.DOWN);
                ArrowManager.Inst.CreateArrowObj(TopBar.Inst.GetIcon(TOPBAR_TYPE.QUESTION).transform.position + new Vector3(0, -1, 0), ArrowCreateDirection.DOWN);
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(2, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());

            if (i == 1 || i == 2 || i == 4)
                TalkWindow.Inst.index2 = i + 1;
            else if (i == 6)
            {
                TurnManager.Inst.isContinue = true;
                ArrowManager.Inst.DestoryAllArrow();
                yield return StartCoroutine(TalkWindow.Inst.HideText());
                TalkWindow.Inst.index = 2;
                TalkWindow.Inst.index2 = 7;
                while (!isWin)
                {
                    if (EnemyManager.Inst.enemys.Count == 0)
                        isWin = true;
                    yield return new WaitForEndOfFrame();
                }
                GhostManager.Inst.MoveTutorialPos();
                yield return StartCoroutine(GhostManager.Inst.ShowGhost());
            }
            else if (i == 7)
            {
                ArrowManager.Inst.DestoryAllArrow();
                while (!isToturialFinish)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return StartCoroutine(TalkWindow.Inst.HideText());
                MapManager.Inst.tutorialIndex++;
                yield return new WaitForSeconds(1);
                GhostManager.Inst.MoveOriginPos();
            }
            yield return null;
        }
    }

    public IEnumerator TutorialSkillCoroutine()      //스킬 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.BATTLE);

        yield return StartCoroutine(StageManager.Inst.CreateStageInTutorial());
        TurnManager.Inst.isTutorialLockCard = true;
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());

        for (int i = 0; i < TalkWindow.Inst.talks[4].Count; i++)
        {
            TalkWindow.Inst.SetFlagIndex(false);
            TalkWindow.Inst.SetFlagNext(false);
            TalkWindow.Inst.SetSkip(false);
            if (i == 0)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).transform.position + new Vector3(0, -1, 0), ArrowCreateDirection.DOWN);
                Debug.Log(CardManager.Inst.MyHandCards.Count);
            }
            else if (i == 1)        //스킬 메뉴 설명
            {
                GhostManager.Inst.MoveTutorialSkillPos();
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(SkillManager.Inst.bookmarks[0].transform.position + new Vector3(-1f, 0, 0), ArrowCreateDirection.LEFT);
            }
            else if (i == 2)        //스킬 설명창 설명
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(new Vector3(-5.25f, -1.5f, -5), ArrowCreateDirection.LEFT);
            }
            else if (i == 3)        //스킬 카드올려두는 공간 설명
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(SkillManager.Inst.ActivePage.choiceCards[0].transform.position + new Vector3(1.3f, 0, 0), ArrowCreateDirection.RIGHT);
            }
            else if (i == 4)
                ArrowManager.Inst.DestoryAllArrow();
            Debug.Log("1");
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(4, i));
            Debug.Log("11");
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            Debug.Log("111");
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
            Debug.Log("1111");
            if (i == 0)
            {
                Debug.Log("2222");
                while (!isToturialOpenSkill)
                {
                    Debug.Log("스킬이 안열렸음");
                    yield return new WaitForEndOfFrame();
                }
                Debug.Log("3333");
            }

        }
        for (int j = 0; j < CardManager.Inst.MyHandCards.Count; j++)
        {
            CardManager.Inst.MyHandCards[j].isLock = false;
        }
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        yield return new WaitForSeconds(1);
        GhostManager.Inst.MoveOriginPos();
    }

    IEnumerator ShowTombCoroutine()
    {
        yield return null;
    }

    IEnumerator BookCoroutine()
    {
        tutorialBook.gameObject.SetActive(true);
        yield return StartCoroutine(tutorialBook.ShowBook());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(tutorialBook.LoadTextTyping(0));
        yield return StartCoroutine(tutorialBook.icons[0].GetComponent<TutorialBookIcon>().SetLook());
        yield return StartCoroutine(CheckGetCardCoroutine());
        yield return StartCoroutine(tutorialBook.LoadTextTyping(1));
        yield return StartCoroutine(tutorialBook.LoadTextTyping(2));
        yield return StartCoroutine(tutorialBook.icons[1].GetComponent<TutorialBookIcon>().SetLook());
        yield return StartCoroutine(CheckGetSkillBookCoroutine());
        yield return StartCoroutine(tutorialBook.LoadTextTyping(3));
        yield return StartCoroutine(tutorialBook.icons[2].GetComponent<TutorialBookIcon>().SetLook());
        yield return StartCoroutine(CheckStartBattleCoroutine());
    }

    public IEnumerator GetCardCoroutine()
    {
        RewardManager.Inst.SetTitleText("무덤");
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 0, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 1, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 2, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 3, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 4, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.CARD, 5, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.REWARD, (int)EVENT_REWARD_TYPE.SKILL_BOOK, 1);
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false));
        yield return StartCoroutine(RewardManager.Inst.RewardCoroutine(false));
        RewardManager.Inst.transform.GetChild(0).gameObject.SetActive(false);
        isGetCard = true;
    }

    IEnumerator GetSkillPageCoroutine()
    {
        yield return null;
    }

    public void Click(TutorialBookIcon icon)
    {
        switch (icon.type)
        {
            case TutorialBookIconType.CARDS:
                StartCoroutine(GetCardCoroutine());
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

    IEnumerator CheckGetCardCoroutine()
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

    IEnumerator CheckGetSkillBookCoroutine()
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

    IEnumerator CheckStartBattleCoroutine()
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
