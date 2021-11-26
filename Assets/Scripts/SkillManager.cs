using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SKILL_TYPE { SKILL1, SKILL2, SKILL3, SKILL4, SKILL5, SKILL6 }
public class SkillManager : MonoBehaviour
{

    public static SkillManager Inst;
    bool isOpen;
    [SerializeField] int page;

    public List<SkillBookPage> pages;
    public List<SkillBookCardButton> bookmarks;
    public List<SkillBookCard> choiceCards;
    public List<SkillBookCard> applyCards;

    public List<GameObject> skillXObjs;
    public bool[] isUseSkill;

    public SkillBookPage ActivePage { get { return this.pages[this.page]; } }
    public int ActivePageIndex { get { return this.page; } }

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
        if (this.isOpen)
        {
            this.Close();
            return;
        }
        SoundManager.Inst.Play(SKILLBOOKSOUND.OPEN_BOOK);
        GameManager.Inst.CloseAllUI();
        this.isOpen = true;
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.SelectPage(this.page);
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
        this.InitCard();
        this.pages[this.page].gameObject.SetActive(false);
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.isOpen = false;
    }

    public void SetCard(SkillBookCard skillBookCard, Card card)
    {
        if (this.isUseSkill[this.page])
            return;
        for (int i = 0; i < this.choiceCards.Count; i++)
        {
            if (this.choiceCards[i].curSelectCard == card)
            {
                this.choiceCards[i].curSelectCard = null;
                this.choiceCards[i].HideCard();
                this.choiceCards[i].SetColorAlpha(true);
                this.choiceCards[i].GetComponentInChildren<TMP_Text>().text = "+";
            }
        }
        SoundManager.Inst.Play(SKILLBOOKSOUND.CARD_ON_BOOK);
        skillBookCard.frontCard.SetActive(true);
        skillBookCard.originNum = card.final_Num;
        skillBookCard.frontCard.GetComponent<SkillBookCard>().originNum = skillBookCard.frontCard.GetComponent<SkillBookCard>().isQuestionMark ? this.RandomNum(card.final_Num) : card.final_Num;
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
        for (int i = 0; i < this.applyCards.Count; i++)
        {
            if (this.applyCards[i].gameObject.activeInHierarchy) this.applyCards[i].curSelectCard.SetFinalNum(this.applyCards[i].curNum);
        }

        this.UseSkill(true);
        this.InitCard();
    }

    public void SelectPage(int index)
    {
        for (int i = 0; i < this.pages.Count; i++)
        {
            if (this.pages[i].gameObject.activeInHierarchy)
            {
                if (i == index)
                    return;
                this.pages[i].Init();
                this.pages[i].gameObject.SetActive(false);
                break;
            }
        }
        SoundManager.Inst.Play(SKILLBOOKSOUND.TURN_PAGE);
        for (int i = 0; i < this.bookmarks.Count; i++)
        {
            //bookmarks[i].gameObject.SetActive(CardManager.Inst.cardDeck[0] >= (i * 2 + 1) ? true : false);
            this.bookmarks[i].gameObject.SetActive(true);
        }

        this.bookmarks[this.page].transform.localPosition += new Vector3(-0.09158f, 0, 0);
        this.page = index;
        this.bookmarks[this.page].transform.localPosition += new Vector3(0.09158f, 0, 0);
        this.pages[this.page].gameObject.SetActive(true);
        this.pages[this.page].Show();
    }

    public void InitCard()
    {
        for (int i = 0; i < this.applyCards.Count; i++)
        {
            if (this.applyCards[i].gameObject.activeInHierarchy) this.applyCards[i].curSelectCard.SetColorAlpha(false);
        }
        for (int i = 0; i < this.choiceCards.Count; i++)
        {
            this.choiceCards[i].curSelectCard = null;
            this.choiceCards[i].HideCard();
            this.choiceCards[i].SetColorAlpha(true);
            this.choiceCards[i].GetComponentInChildren<TMP_Text>().text = "+";
        }
    }

    public void InitSkillTime()
    {
        for (int i = 0; i < this.isUseSkill.Length; i++)
        {
            this.isUseSkill[i] = false;
            this.skillXObjs[i].SetActive(false);
        }
    }

    public void UseSkill(bool use)
    {
        this.isUseSkill[this.page] = use;
        this.skillXObjs[this.page].SetActive(use);
    }
}
