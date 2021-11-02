using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Inst = null;
    bool isOpen = false;
    int page = 0;

    public List<SkillBookPage> pages;
    public List<SkillBookCardButton> bookmarks;
    public List<SkillBookCard> choiceCards;
    public List<SkillBookCard> applyCards;

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
        GameManager.Inst.CloseAllUI();
        isOpen = true;
        transform.GetChild(0).gameObject.SetActive(true);
        SelectPage(page);
    }

    public void Close()      //스킬창 여는 것
    {
        InitCard();
        transform.GetChild(0).gameObject.SetActive(false);
        isOpen = false;
    }

    public void SetCard(SkillBookCard skillBookCard, Card card)
    {
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
        skillBookCard.frontCard.SetActive(true);
        skillBookCard.curNum = card.final_Num;
        skillBookCard.frontCard.GetComponent<SkillBookCard>().curNum = card.final_Num;
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

    public void ApplyCardAll()
    {
        for (int i = 0; i < applyCards.Count; i++)
        {
            if (applyCards[i].gameObject.activeInHierarchy)
                applyCards[i].curSelectCard.SetFinalNum(applyCards[i].curNum);
        }
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
}
