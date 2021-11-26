using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBookPage : MonoBehaviour
{
    public SKILL_TYPE skill_type;
    public List<SkillBookCard> choiceCards;
    public List<SkillBookCard> applyCards;

    [SerializeField] List<TMP_Text> TextTMP;
    [SerializeField] List<SpriteRenderer> renderers;

    public SkillBookCardButton applyButton;

    public bool isFinishFade;

    public void Init()
    {
        for (int i = 0; i < this.choiceCards.Count; i++)
        {
            if (this.choiceCards[i].curSelectCard != null)
            {
                this.choiceCards[i].curSelectCard.SetColorAlpha(false);
                this.choiceCards[i].curSelectCard = null;
                this.choiceCards[i].HideCard();
                this.choiceCards[i].SetColorAlpha(true);
                this.choiceCards[i].GetComponentInChildren<TMP_Text>().text = "+";
            }
        }

        this.applyButton.isActive = false;
    }
    public void Show()
    {
        this.Init();
        this.isFinishFade = false;
        SkillManager.Inst.choiceCards = this.choiceCards;
        SkillManager.Inst.applyCards = this.applyCards;
        for (int i = 0; i < this.TextTMP.Count; i++)
        {
            this.TextTMP[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < this.renderers.Count; i++)
        {
            this.renderers[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < this.choiceCards.Count; i++)
        {
            this.choiceCards[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            this.choiceCards[i].GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 0);
        }
        switch (this.skill_type)
        {
            case SKILL_TYPE.SKILL1:     //한장의 카드에 +1 or -1
                for (int i = 0; i < this.applyCards.Count; i++)
                {
                    this.applyCards[i].limitNum = 1;
                }
                break;
            case SKILL_TYPE.SKILL2:     //-n +n
                for (int i = 0; i < this.applyCards.Count; i++)
                {
                    this.applyCards[i].isHideButton = true;
                    this.applyCards[i].isQuestionMark = true;
                }
                break;
            case SKILL_TYPE.SKILL3:     //원하는 숫자로 카드 한장 바꾸기
                break;
            case SKILL_TYPE.SKILL4:     //최대 3장 선택 후 +1 -1
                for (int i = 0; i < this.applyCards.Count; i++)
                {
                    this.applyCards[i].limitNum = 1;
                }
                break;
            case SKILL_TYPE.SKILL5:     //손패에 있는 카드 한장을 다른 카드에 복제
                for (int i = 0; i < this.applyCards.Count; i++)
                {
                    this.applyCards[i].isHideButton = true;
                    //applyCards[i].isApplyButtonOn = true;
                }
                break;
            case SKILL_TYPE.SKILL6:     //최대 3장 선택후 랜덤 숫자로 변경
                for (int i = 0; i < this.applyCards.Count; i++)
                {
                    this.applyCards[i].isHideButton = true;
                    this.applyCards[i].isQuestionMark = true;
                }
                break;
        }
        //applyButton.SetButtonActive(false);
        this.StartCoroutine(this.ColorAlphaCoroutine(false));
    }
    public void Hide()
    {
        this.StartCoroutine(this.ColorAlphaCoroutine(true));
    }

    IEnumerator ColorAlphaCoroutine(bool isHide)
    {
        while (true)
        {
            float alpha = Time.deltaTime * (isHide ? -1 : 1);
            for (int i = 0; i < this.TextTMP.Count; i++)
            {
                this.TextTMP[i].color += Color.black * alpha;
            }
            for (int i = 0; i < this.renderers.Count; i++)
            {
                this.renderers[i].color += Color.black * alpha;
            }

            this.applyButton.GetComponent<SpriteRenderer>().color -= Color.black * alpha * 0.5f;
            this.applyButton.GetComponentInChildren<TMP_Text>().color -= Color.black * alpha * 0.5f;
            for (int i = 0; i < this.choiceCards.Count; i++)
            {
                this.choiceCards[i].GetComponent<SpriteRenderer>().color += Color.black * alpha * (this.choiceCards[i].curSelectCard == null ? 0.5f : 1);
                this.choiceCards[i].GetComponentInChildren<TMP_Text>().color += Color.black * alpha * (this.choiceCards[i].curSelectCard == null ? 0.5f : 1);
            }
            if (isHide)
            {
                for (int i = 0; i < this.applyCards.Count; i++)
                {
                    this.choiceCards[i].GetComponent<SpriteRenderer>().color += Color.black * alpha;
                    this.choiceCards[i].GetComponentInChildren<TMP_Text>().color += Color.black * alpha;
                }
                if (this.TextTMP[0].color.a <= 0)
                    break;
            }
            else
            {
                if (this.TextTMP[0].color.a >= 1)
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
        if (isHide)
            this.gameObject.SetActive(false);
        else
            this.applyButton.SetButtonActive(false);
        this.isFinishFade = true;
    }
}
