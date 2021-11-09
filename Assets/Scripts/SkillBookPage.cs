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

    public void Init()
    {
        for (int i = 0; i < choiceCards.Count; i++)
        {
            if (choiceCards[i].curSelectCard != null)
            {
                choiceCards[i].curSelectCard.SetColorAlpha(false);
                choiceCards[i].curSelectCard = null;
                choiceCards[i].HideCard();
                choiceCards[i].SetColorAlpha(true);
                choiceCards[i].GetComponentInChildren<TMP_Text>().text = "+";
            }
        }
        applyButton.isActive = false;
    }
    public void Show()
    {
        Init();
        SkillManager.Inst.choiceCards = this.choiceCards;
        SkillManager.Inst.applyCards = this.applyCards;
        for (int i = 0; i < TextTMP.Count; i++)
        {
            TextTMP[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < choiceCards.Count; i++)
        {
            choiceCards[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            choiceCards[i].GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 0);
        }
        switch (skill_type)
        {
            case SKILL_TYPE.SKILL1:     //한장의 카드에 +1 or -1
                for (int i = 0; i < applyCards.Count; i++)
                {
                    applyCards[i].limitNum = 1;
                }
                break;
            case SKILL_TYPE.SKILL2:     //-n +n
                for (int i = 0; i < applyCards.Count; i++)
                {
                    applyCards[i].isHideButton = true;
                    applyCards[i].isQuestionMark = true;
                }
                break;
            case SKILL_TYPE.SKILL3:     //원하는 숫자로 카드 한장 바꾸기
                break;
            case SKILL_TYPE.SKILL4:     //최대 3장 선택 후 +1 -1
                for (int i = 0; i < applyCards.Count; i++)
                {
                    applyCards[i].limitNum = 1;
                }
                break;
            case SKILL_TYPE.SKILL5:     //손패에 있는 카드 한장을 다른 카드에 복제
                for (int i = 0; i < applyCards.Count; i++)
                {
                    applyCards[i].isHideButton = true;
                    //applyCards[i].isApplyButtonOn = true;
                }
                break;
            case SKILL_TYPE.SKILL6:     //최대 3장 선택후 랜덤 숫자로 변경
                for (int i = 0; i < applyCards.Count; i++)
                {
                    applyCards[i].isHideButton = true;
                    applyCards[i].isQuestionMark = true;
                }
                break;
            default:
                break;
        }
        //applyButton.SetButtonActive(false);
        StartCoroutine(ColorAlphaCoroutine(false));
    }
    public void Hide()
    {
        StartCoroutine(ColorAlphaCoroutine(true));
    }

    IEnumerator ColorAlphaCoroutine(bool isHide)
    {
        while (true)
        {
            float alpha = Time.deltaTime * (isHide ? -1 : 1);
            for (int i = 0; i < TextTMP.Count; i++)
            {
                TextTMP[i].color += Color.black * alpha;
            }
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].color += Color.black * alpha;
            }
            applyButton.GetComponent<SpriteRenderer>().color -= Color.black * alpha * 0.5f;
            applyButton.GetComponentInChildren<TMP_Text>().color -= Color.black * alpha * 0.5f;
            for (int i = 0; i < choiceCards.Count; i++)
            {
                choiceCards[i].GetComponent<SpriteRenderer>().color += Color.black * alpha * (choiceCards[i].curSelectCard == null ? 0.5f : 1);
                choiceCards[i].GetComponentInChildren<TMP_Text>().color += Color.black * alpha * (choiceCards[i].curSelectCard == null ? 0.5f : 1);
            }
            if (isHide)
            {
                for (int i = 0; i < applyCards.Count; i++)
                {
                    choiceCards[i].GetComponent<SpriteRenderer>().color += Color.black * alpha;
                    choiceCards[i].GetComponentInChildren<TMP_Text>().color += Color.black * alpha;
                }
                if (TextTMP[0].color.a <= 0)
                    break;
            }
            else
            {
                if (TextTMP[0].color.a >= 1)
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
        if (isHide)
            this.gameObject.SetActive(false);
        else
            applyButton.SetButtonActive(false);
    }
}
