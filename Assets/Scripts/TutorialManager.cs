using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private HpBar playerHpBar;
    [SerializeField] private TutorialBook tutorialBook;

    private bool isGetCard;
    private bool isGetSkillBook;
    private bool isStartBattle;

    public static TutorialManager Inst;
    [SerializeField] private Tomb tomb;

    public bool isWin;

    public bool isToturialFinish;
    public bool isToturialOpenSkill;

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
            case 3:
                StartCoroutine(TutorialSkillCoroutine());
                break;
        }
    }

    public IEnumerator TutorialStoryCoroutine()      //기본 스토리 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.Tutorial);
        playerHpBar.SetHP(60);
        //
        //yield return new WaitForSeconds(2);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[0].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, 0, 0), ArrowCreateDirection.Right);
            }
            else if (i == 4)
            {
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, 0, 0), ArrowCreateDirection.Right);
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(TalkWindow.Inst.index, TalkWindow.Inst.index2));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
            if (i == 3)
            {
                ArrowManager.Inst.DestroyAllArrow();
                //무덤 이미지 보이게 하기
                tomb.gameObject.SetActive(true);
                yield return StartCoroutine(tomb.SetLook());
                ArrowManager.Inst.CreateArrowObj(tomb.transform.position + new Vector3(1.5f, 0, 0), ArrowCreateDirection.Right);
                yield return StartCoroutine(CheckGetCardCoroutine());
                yield return StartCoroutine(CheckGetSkillBookCoroutine());
                ArrowManager.Inst.DestroyAllArrow();
            }
        }
        ArrowManager.Inst.DestroyAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        MapManager.Inst.LoadMapScene(true);
        MapManager.Inst.tutorialIndex++;
        yield return null;
    }

    public IEnumerator TutorialBattleCoroutine()      //기본 전투 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.Battle);

        yield return StartCoroutine(StageManager.Inst.CreateStageInTutorial());
        TurnManager.Inst.isTutorialLockCard = true;
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());

        for (int i = 0; i < TalkWindow.Inst.talks[2].Count; i++)
        {
            if (i == 0)     //앞에 몬스터 머리 위에 있는 3이라는 숫자가 보이나? 그 숫자가 적의 약점 숫자라네.
            {
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].weaknessTMP.transform.position + new Vector3(0, 1f, 0), ArrowCreateDirection.Up);
            }
            else if (i == 1 || i == 2)  //손에 들고 있는 3 카드로 한번 공격해보게. 그 숫자 그대로 데미지가 들어갈 거야., 이번엔 6 카드로 한번 공격해봐. 약점 숫자랑 다르다면 데미지가 1밖에 안 들어갈 거네.
            {
                ArrowManager.Inst.DestroyAllArrow();
                CardManager.Inst.UnLockMyHandCard(0);
                ArrowManager.Inst.CreateArrowObj(CardManager.Inst.MyHandCards[0].transform.position + new Vector3(0, 2, 0), ArrowCreateDirection.Up, CardManager.Inst.MyHandCards[0].transform);
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].hitPos.transform.position + new Vector3(-2, 0, 0), ArrowCreateDirection.Left);
                TalkWindow.Inst.SetFlagIndex(true);
                CardManager.Inst.isTutorial = true;
            }
            else if (i == 3)    //약점 숫자 뒤에 아이콘은 적의 패턴이라네. 검일 땐 공격, 십자가일 땐 회복이지.
            {
                ArrowManager.Inst.DestroyAllArrow();
                ArrowManager.Inst.CreateArrowObj(EnemyManager.Inst.enemys[0].pattenIndexTMP.transform.position + new Vector3(-0.5f, 1, 0), ArrowCreateDirection.Up);
            }
            else if (i == 4)    //나머지 카드를 자네에게 써보게. 그렇다면 해당 숫자만큼의 실드가 생길 거야
            {
                ArrowManager.Inst.DestroyAllArrow();
                CardManager.Inst.UnLockMyHandCard(0);
                ArrowManager.Inst.CreateArrowObj(CardManager.Inst.MyHandCards[0].transform.position + new Vector3(0, 2f, 0), ArrowCreateDirection.Up, CardManager.Inst.MyHandCards[0].transform);
                ArrowManager.Inst.CreateArrowObj(Player.Inst.transform.position + new Vector3(1.5f, 1, 0), ArrowCreateDirection.Right);
                TalkWindow.Inst.SetFlagIndex(true);
                TurnManager.Inst.isContinue = false;
            }
            else if (i == 5)    //실드는 적의 공격을 실드 숫자만큼 방어해주지.
            {
                ArrowManager.Inst.DestroyAllArrow();
                ArrowManager.Inst.CreateArrowObj(Player.Inst.hpbar.sheldtext.transform.position + new Vector3(-1, 0, 0), ArrowCreateDirection.Left);
                CardManager.Inst.isTutorial = false;
            }
            else if (i == 6)    //그리고 손에 들고 있는 카드를 다 사용하면 몬스터의 턴으로 넘어가게 된다네. 계속 싸워서 이겨보게나.
            {
                ArrowManager.Inst.DestroyAllArrow();
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, 0, 0), ArrowCreateDirection.Right);
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(2, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());

            if (i == 1 || i == 2 || i == 4)
            {
                TalkWindow.Inst.index2 = i + 1;
            }
            else if (i == 6)
            {
                TurnManager.Inst.isContinue = true;
                ArrowManager.Inst.DestroyAllArrow();
                MapManager.Inst.tutorialIndex++;
                yield return StartCoroutine(TalkWindow.Inst.HideText());
            }
            yield return null;
        }
    }

    public IEnumerator TutorialSkillCoroutine()      //스킬 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.Battle);

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
                ArrowManager.Inst.DestroyAllArrow();
                ArrowManager.Inst.CreateArrowObj(TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).transform.position + new Vector3(0, -1, 0), ArrowCreateDirection.Down);
            }
            else if (i == 1)        //스킬 메뉴 설명
            {
                GhostManager.Inst.MoveTutorialSkillPos();
                ArrowManager.Inst.DestroyAllArrow();
                ArrowManager.Inst.CreateArrowObj(SkillManager.Inst.bookmarks[0].transform.position + new Vector3(-1f, 0, 0), ArrowCreateDirection.Left);
            }
            else if (i == 2)        //스킬 설명창 설명
            {
                ArrowManager.Inst.DestroyAllArrow();
                ArrowManager.Inst.CreateArrowObj(new Vector3(-5.25f, -1.5f, -5), ArrowCreateDirection.Left);
            }
            else if (i == 3)        //스킬 카드올려두는 공간 설명
            {
                ArrowManager.Inst.DestroyAllArrow();
                ArrowManager.Inst.CreateArrowObj(SkillManager.Inst.ActivePage.choiceCards[0].transform.position + new Vector3(1.3f, 0, 0), ArrowCreateDirection.Right);
            }
            else if (i == 4)
            {
                ArrowManager.Inst.DestroyAllArrow();
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(4, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());

            if (i == 0)
            {
                while (!isToturialOpenSkill)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

        }

        CardManager.Inst.UnLockMyHandCardAll();

        yield return StartCoroutine(TalkWindow.Inst.HideText());

        yield return new WaitForSeconds(1);

        GhostManager.Inst.MoveOriginPos();
    }

    private IEnumerator ShowTombCoroutine()
    {
        yield return null;
    }

    private IEnumerator BookCoroutine()
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
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int)EVENT_REWARD_TYPE.Card, 0, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int)EVENT_REWARD_TYPE.Card, 1, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int)EVENT_REWARD_TYPE.Card, 2, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int)EVENT_REWARD_TYPE.Card, 3, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int)EVENT_REWARD_TYPE.Card, 4, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int)EVENT_REWARD_TYPE.Card, 5, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int)EVENT_REWARD_TYPE.SkillBook, 1);
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false));
        yield return StartCoroutine(RewardManager.Inst.RewardCoroutine(false));
        RewardManager.Inst.transform.GetChild(0).gameObject.SetActive(false);
        isGetCard = true;
    }

    private IEnumerator GetSkillPageCoroutine()
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
                ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.SkillBook, icon.transform.position, TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).transform.position, SetActiveTrueTopBarSkillBook());
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

    private IEnumerator CheckGetCardCoroutine()
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

    private IEnumerator CheckGetSkillBookCoroutine()
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

    private IEnumerator CheckStartBattleCoroutine()
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
