using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SKILL_TYPE { SKILL1, SKILL2, SKILL3, SKILL4, SKILL5, SKILL6 }
public class SkillManager : MonoBehaviour
{

    public static SkillManager Inst = null;
    bool isOpen = false;
    [SerializeField] int page = 0;

    public List<SkillBookPage> pages;
    public List<SkillBookCardButton> bookmarks;
    public List<SkillBookCard> choiceCards;
    public List<SkillBookCard> applyCards;

    public List<GameObject> skillXObjs;
    public bool[] isUseSkill;

    public SkillBookPage ActivePage { get { return pages[page]; } }
    public int ActivePageIndex { get { return page; } }

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Init()
    {
        this.transform.position = new Vector3(0, 0, -2);
    }

    public void Open()      //스킬창 여는 것
    {
        if (isOpen)
        {
            Close();
            return;
        }
        SoundManager.Inst.Play(SKILLBOOKSOUND.OPEN_BOOK);
        GameManager.Inst.CloseAllUI();
        isOpen = true;
        transform.GetChild(0).gameObject.SetActive(true);
        SelectPage(page);
        if (MapManager.Inst.tutorialIndex == 3 && SceneManager.GetActiveScene().name == "Tutorial2")
        {
            TutorialManager.Inst.isToturialOpenSkill = true;
            TalkWindow.Inst.SetFlagNext(true);
            TalkWindow.Inst.SetSkip(true);
            TalkWindow.Inst.index2 = 1;
        }
    }

    public void Close()      //스킬창 여는 것
    {
        SoundManager.Inst.Play(SKILLBOOKSOUND.CLOSE_BOOK);
        InitCard();
        pages[page].gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        isOpen = false;
    }

    public void SetCard(SkillBookCard skillBookCard, Card card)
    {
        if (isUseSkill[page])
            return;
        for (int i = 0; i < choiceCards.Count; i++)
        {
            if (choiceCards[i].curSelectCard == card)
            {
                choiceCards[i].curSelectCard = null;
                choiceCards[i].HideCard();
                choiceCards[i].SetColorAlpha(true);
                choiceCards[i].GetComponentInChildren<TMP_Text>().text = "+";
            }
        }
        SoundManager.Inst.Play(SKILLBOOKSOUND.CARD_ON_BOOK);
        skillBookCard.frontCard.SetActive(true);
        skillBookCard.originNum = card.final_Num;
        skillBookCard.frontCard.GetComponent<SkillBookCard>().originNum = skillBookCard.frontCard.GetComponent<SkillBookCard>().isQuestionMark ? RandomNum(card.final_Num) : card.final_Num;
        skillBookCard.SetColorAlpha(false);
        skillBookCard.GetComponentInChildren<TMP_Text>().text = (card.final_Num + 1).ToString();
        if (skillBookCard.curSelectCard == null)
        {
            card.SetColorAlpha(true);
            skillBookCard.curSelectCard = card;
            skillBookCard.frontCard.GetComponent<SkillBookCard>().curSelectCard = card;
        }
        else
        {
            skillBookCard.curSelectCard.SetColorAlpha(false);
            skillBookCard.curSelectCard = card;
            skillBookCard.curSelectCard.SetColorAlpha(true);
            skillBookCard.frontCard.GetComponent<SkillBookCard>().curSelectCard = card;
        }
    }

    int RandomNum(int num)
    {
        int result = 0;
        do
        {
            result = Random.Range(0, 6);
        } while (num == result);
        return result;
    }

    public void ApplyCardAll()
    {
        for (int i = 0; i < applyCards.Count; i++)
        {
            if (applyCards[i].gameObject.activeInHierarchy)
                applyCards[i].curSelectCard.SetFinalNum(applyCards[i].curNum);
        }
        UseSkill(true);
        InitCard();
    }

    public void SelectPage(int index)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            if (pages[i].gameObject.activeInHierarchy)
            {
                if (i == index)
                    return;
                pages[i].Init();
                pages[i].gameObject.SetActive(false);
                break;
            }
        }
        SoundManager.Inst.Play(SKILLBOOKSOUND.TURN_PAGE);
        for (int i = 0; i < bookmarks.Count; i++)
        {
            //bookmarks[i].gameObject.SetActive(CardManager.Inst.cardDeck[0] >= (i * 2 + 1) ? true : false);
            bookmarks[i].gameObject.SetActive(true);
        }
        bookmarks[page].transform.localPosition += new Vector3(-0.09158f, 0, 0);
        page = index;
        bookmarks[page].transform.localPosition += new Vector3(0.09158f, 0, 0);
        pages[page].gameObject.SetActive(true);
        pages[page].Show();
    }

    public void InitCard()
    {
        for (int i = 0; i < applyCards.Count; i++)
        {
            if (applyCards[i].gameObject.activeInHierarchy)
                applyCards[i].curSelectCard.SetColorAlpha(false);
        }
        for (int i = 0; i < choiceCards.Count; i++)
        {
            choiceCards[i].curSelectCard = null;
            choiceCards[i].HideCard();
            choiceCards[i].SetColorAlpha(true);
            choiceCards[i].GetComponentInChildren<TMP_Text>().text = "+";
        }
    }

    public void InitSkillTime()
    {
        for (int i = 0; i < isUseSkill.Length; i++)
        {
            isUseSkill[i] = false;
            skillXObjs[i].SetActive(false);
        }
    }

    public void UseSkill(bool use)
    {
        isUseSkill[page] = use;
        skillXObjs[page].SetActive(use);
    }
}
