using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Inst = null;
    bool isOpen = false;

    public List<SkillBookCard> skillBookCards;
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
    }

    public void Close()      //스킬창 여는 것
    {
        InitCard();
        transform.GetChild(0).gameObject.SetActive(false);
        isOpen = false;
    }

    public void SetCard(SkillBookCard skillBookCard, Card card)
    {
        for (int i = 0; i < skillBookCards.Count; i++)
        {
            if (skillBookCards[i].curSelectCard == card)
            {
                skillBookCards[i].curSelectCard = null;
                skillBookCards[i].HideCard();
                skillBookCards[i].SetColorAlpha(true);
                skillBookCards[i].GetComponentInChildren<TMP_Text>().text = "+";
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

    public void InitCard()
    {
        for (int i = 0; i < applyCards.Count; i++)
        {
            if (applyCards[i].gameObject.activeInHierarchy)
                applyCards[i].curSelectCard.SetColorAlpha(false);
        }
        for (int i = 0; i < skillBookCards.Count; i++)
        {
            skillBookCards[i].curSelectCard = null;
            skillBookCards[i].HideCard();
            skillBookCards[i].SetColorAlpha(true);
            skillBookCards[i].GetComponentInChildren<TMP_Text>().text = "+";
        }
    }
}
