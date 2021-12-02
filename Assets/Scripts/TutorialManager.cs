using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private HpBar playerHpBar;

    private bool isGetCard;
    private bool isGetSkillBook;
    private bool isStartBattle;

    public static TutorialManager Inst;
    [SerializeField] private Tomb tomb;

    public bool isToturialOpenSkill;

    private readonly WaitForEndOfFrame waitEndFrame = new WaitForEndOfFrame();

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

    private IEnumerator TutorialStoryCoroutine() //기본 스토리 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.Tutorial);
        playerHpBar.SetHp(60);

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (var i = 0; i < TalkWindow.Inst.TalkLists[0].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, 0, 0),
                    ArrowCreateDirection.Right);
            }
            else if (i == 4)
            {
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, 0, 0),
                    ArrowCreateDirection.Right);
            }

            yield return StartCoroutine(
                TalkWindow.Inst.TalkTypingCoroutine(TalkWindow.Inst.Index, TalkWindow.Inst.Index2));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
            if (i == 3)
            {
                ArrowManager.Inst.DestroyAllArrow();
                //무덤 이미지 보이게 하기
                tomb.gameObject.SetActive(true);
                yield return StartCoroutine(tomb.SetLook());
                ArrowManager.Inst.CreateArrowObj(tomb.transform.position + new Vector3(1.5f, 0, 0),
                    ArrowCreateDirection.Right);
                yield return StartCoroutine(CheckGetCardAndSkillBookCoroutine());
                ArrowManager.Inst.DestroyAllArrow();
            }
        }

        ArrowManager.Inst.DestroyAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        MapManager.Inst.LoadMapScene(true);
        MapManager.Inst.tutorialIndex++;
        yield return null;
    }

    private IEnumerator CheckGetCardAndSkillBookCoroutine()
    {
        while (true)
        {
            if ((CardManager.Inst.cardDeck[0] == 1) && (CardManager.Inst.cardDeck[1] == 2) &&
                (CardManager.Inst.cardDeck[2] == 3) && (CardManager.Inst.cardDeck[3] == 3) &&
                (CardManager.Inst.cardDeck[4] == 2) && (CardManager.Inst.cardDeck[5] == 1))
            {
                if (TopBarManager.Inst.GetIcon(TOPBAR_TYPE.Skill).gameObject.activeInHierarchy)
                {
                    break;
                }
            }

            yield return waitEndFrame;
        }

        yield return null;
    }

    public IEnumerator TutorialBattleCoroutine() //기본 전투 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.Battle);

        yield return StartCoroutine(StageManager.Inst.CreateStageInTutorial());
        TurnManager.Inst.isTutorialLockCard = true;
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());

        for (var i = 0; i < TalkWindow.Inst.TalkLists[2].Count; i++)
        {
            switch (i)
            {
                //앞에 몬스터 머리 위에 있는 3이라는 숫자가 보이나? 그 숫자가 적의 약점 숫자라네.
                case 0:
                    ArrowManager.Inst.CreateArrowObj(
                        EnemyManager.Inst.enemys[0].weaknessTMP.transform.position + new Vector3(0, 1f, 0),
                        ArrowCreateDirection.Up);
                    break;
                case 1:
                //손에 들고 있는 3 카드로 한번 공격해보게. 그 숫자 그대로 데미지가 들어갈 거야., 이번엔 6 카드로 한번 공격해봐. 약점 숫자랑 다르다면 데미지가 1밖에 안 들어갈 거네.
                case 2:
                    ArrowManager.Inst.DestroyAllArrow();
                    CardManager.Inst.UnLockMyHandCard(0);
                    ArrowManager.Inst.CreateArrowObj(
                        CardManager.Inst.MyHandCards[0].transform.position + new Vector3(0, 2, 0),
                        ArrowCreateDirection.Up,
                        CardManager.Inst.MyHandCards[0].transform);
                    ArrowManager.Inst.CreateArrowObj(
                        EnemyManager.Inst.enemys[0].hitPos.transform.position + new Vector3(-2, 0, 0),
                        ArrowCreateDirection.Left);
                    TalkWindow.Inst.SetFlagIndex(true);
                    CardManager.Inst.isTutorial = true;
                    break;
                //약점 숫자 뒤에 아이콘은 적의 패턴이라네. 검일 땐 공격, 십자가일 땐 회복이지.
                case 3:
                    ArrowManager.Inst.DestroyAllArrow();
                    ArrowManager.Inst.CreateArrowObj(
                        EnemyManager.Inst.enemys[0].pattenIndexTMP.transform.position + new Vector3(-0.5f, 1, 0),
                        ArrowCreateDirection.Up);
                    break;
                //나머지 카드를 자네에게 써보게. 그렇다면 해당 숫자만큼의 실드가 생길 거야
                case 4:
                    ArrowManager.Inst.DestroyAllArrow();
                    CardManager.Inst.UnLockMyHandCard(0);
                    ArrowManager.Inst.CreateArrowObj(
                        CardManager.Inst.MyHandCards[0].transform.position + new Vector3(0, 2f, 0),
                        ArrowCreateDirection.Up,
                        CardManager.Inst.MyHandCards[0].transform);
                    ArrowManager.Inst.CreateArrowObj(Player.Inst.transform.position + new Vector3(1.5f, 1, 0),
                        ArrowCreateDirection.Right);
                    TalkWindow.Inst.SetFlagIndex(true);
                    TurnManager.Inst.isContinue = false;
                    break;
                //실드는 적의 공격을 실드 숫자만큼 방어해주지.
                case 5:
                    ArrowManager.Inst.DestroyAllArrow();
                    ArrowManager.Inst.CreateArrowObj(
                        Player.Inst.hpbar.sheldtext.transform.position + new Vector3(-1, 0, 0),
                        ArrowCreateDirection.Left);
                    CardManager.Inst.isTutorial = false;
                    break;
                //그리고 손에 들고 있는 카드를 다 사용하면 몬스터의 턴으로 넘어가게 된다네. 계속 싸워서 이겨보게나.
                case 6:
                    ArrowManager.Inst.DestroyAllArrow();
                    ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, 0, 0),
                        ArrowCreateDirection.Right);
                    break;
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(2, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());

            switch (i)
            {
                case 1:
                case 2:
                case 4:
                    TalkWindow.Inst.Index2 = i + 1;
                    break;
                case 6:
                    TurnManager.Inst.isContinue = true;
                    ArrowManager.Inst.DestroyAllArrow();
                    MapManager.Inst.tutorialIndex++;
                    yield return StartCoroutine(TalkWindow.Inst.HideText());
                    break;
            }

            yield return null;
        }
    }

    private IEnumerator TutorialSkillCoroutine() //스킬 설명
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.Battle);

        yield return StartCoroutine(StageManager.Inst.CreateStageInTutorial());
        TurnManager.Inst.isTutorialLockCard = true;
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());

        for (var i = 0; i < TalkWindow.Inst.TalkLists[4].Count; i++)
        {
            TalkWindow.Inst.SetFlagIndex(false);
            TalkWindow.Inst.SetFlagNext(false);
            TalkWindow.Inst.SetSkip(false);
            switch (i)
            {
                case 0:
                    ArrowManager.Inst.DestroyAllArrow();
                    ArrowManager.Inst.CreateArrowObj(
                        TopBarManager.Inst.GetIcon(TOPBAR_TYPE.Skill).transform.position + new Vector3(0, -1, 0),
                        ArrowCreateDirection.Down);
                    break;
                //스킬 메뉴 설명
                case 1:
                    GhostManager.Inst.MoveTutorialSkillPos();
                    ArrowManager.Inst.DestroyAllArrow();
                    ArrowManager.Inst.CreateArrowObj(
                        SkillManager.Inst.bookmarks[0].transform.position + new Vector3(-1f, 0, 0),
                        ArrowCreateDirection.Left);
                    break;
                //스킬 설명창 설명
                case 2:
                    ArrowManager.Inst.DestroyAllArrow();
                    ArrowManager.Inst.CreateArrowObj(new Vector3(-5.25f, -1.5f, -5), ArrowCreateDirection.Left);
                    break;
                //스킬 카드올려두는 공간 설명
                case 3:
                    ArrowManager.Inst.DestroyAllArrow();
                    ArrowManager.Inst.CreateArrowObj(
                        SkillManager.Inst.ActivePage.choiceCards[0].transform.position + new Vector3(1.3f, 0, 0),
                        ArrowCreateDirection.Right);
                    break;
                case 4:
                    ArrowManager.Inst.DestroyAllArrow();
                    break;
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(4, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());

            if (i == 0)
            {
                while (!isToturialOpenSkill)
                {
                    yield return waitEndFrame;
                }
            }
        }

        CardManager.Inst.UnLockMyHandCardAll();

        yield return StartCoroutine(TalkWindow.Inst.HideText());

        yield return new WaitForSeconds(1);

        GhostManager.Inst.MoveOriginPos();
    }

    public IEnumerator GetCardCoroutine()
    {
        RewardManager.Inst.SetTitleText("무덤");
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.Card, 0, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.Card, 1, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.Card, 2, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.Card, 3, 3);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.Card, 4, 2);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.Card, 5, 1);
        RewardManager.Inst.AddReward(REWARD_TYPE.Reward, (int) EVENT_REWARD_TYPE.SkillBook, 1);
        yield return StartCoroutine(RewardManager.Inst.ShowRewardWindowCoroutine(false));
        yield return StartCoroutine(RewardManager.Inst.RewardCoroutine(false));
        RewardManager.Inst.transform.GetChild(0).gameObject.SetActive(false);
        isGetCard = true;
    }

    public IEnumerator SetActiveTrueTopBarSkillBook()
    {
        TopBarManager.Inst.GetIcon(TOPBAR_TYPE.Skill).gameObject.SetActive(true);
        isGetSkillBook = true;
        yield return null;
    }
}
