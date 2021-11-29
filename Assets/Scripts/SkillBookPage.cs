using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBookPage : MonoBehaviour
{
    public SKILL_TYPE skill_type;
    public List<SkillBookCard> choiceCards;
    public List<SkillBookCard> applyCards;

    [SerializeField] private List<TMP_Text> TextTMP;
    [SerializeField] private List<SpriteRenderer> renderers;

    public SkillBookCardButton applyButton;

    public bool isFinishFade;

    public void Init()
    {
        foreach (SkillBookCard choiceCard in choiceCards)
        {
            if (choiceCard.curSelectCard == null)
            {
                continue;
            }

            choiceCard.curSelectCard.SetColorAlpha(false);
            choiceCard.curSelectCard = null;
            choiceCard.HideCard();
            choiceCard.SetColorAlpha(true);
            choiceCard.GetComponentInChildren<TMP_Text>().text = "+";
        }

        applyButton.isActive = false;
    }

    public void Show()
    {
        Init();
        isFinishFade = false;
        SkillManager.Inst.choiceCards = choiceCards;
        SkillManager.Inst.applyCards = applyCards;
        foreach (TMP_Text textTmp in TextTMP)
        {
            textTmp.color = new Color(1, 1, 1, 0);
        }

        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = new Color(1, 1, 1, 0);
        }

        foreach (SkillBookCard choiceCard in choiceCards)
        {
            choiceCard.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            choiceCard.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 0);
        }

        switch (skill_type)
        {
            case SKILL_TYPE.Skill1: //한장의 카드에 +1 or -1
                foreach (SkillBookCard applyCard in applyCards)
                {
                    applyCard.limitNum = 1;
                }

                break;
            case SKILL_TYPE.Skill2: //-n +n
                foreach (SkillBookCard applyCard in applyCards)
                {
                    applyCard.isHideButton = true;
                    applyCard.isQuestionMark = true;
                }

                break;
            case SKILL_TYPE.Skill3: //원하는 숫자로 카드 한장 바꾸기
                break;
            case SKILL_TYPE.Skill4: //최대 3장 선택 후 +1 -1
                foreach (SkillBookCard applyCard in applyCards)
                {
                    applyCard.limitNum = 1;
                }

                break;
            case SKILL_TYPE.Skill5: //손패에 있는 카드 한장을 다른 카드에 복제
                foreach (SkillBookCard applyCard in applyCards)
                {
                    applyCard.isHideButton = true;
                }

                break;
            case SKILL_TYPE.Skill6: //최대 3장 선택후 랜덤 숫자로 변경
                foreach (SkillBookCard applyCard in applyCards)
                {
                    applyCard.isHideButton = true;
                    applyCard.isQuestionMark = true;
                }

                break;
        }

        StartCoroutine(ColorAlphaCoroutine(false));
    }

    private IEnumerator ColorAlphaCoroutine(bool isHide)
    {
        while (true)
        {
            float alpha = Time.deltaTime * (isHide ? -1 : 1);
            foreach (TMP_Text textTmp in TextTMP)
            {
                textTmp.color += Color.black * alpha;
            }

            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.color += Color.black * alpha;
            }

            applyButton.GetComponent<SpriteRenderer>().color -= Color.black * alpha * 0.5f;
            applyButton.GetComponentInChildren<TMP_Text>().color -= Color.black * alpha * 0.5f;
            foreach (SkillBookCard choiceCard in choiceCards)
            {
                choiceCard.GetComponent<SpriteRenderer>().color +=
                    Color.black * alpha * (choiceCard.curSelectCard == null ? 0.5f : 1);
                choiceCard.GetComponentInChildren<TMP_Text>().color +=
                    Color.black * alpha * (choiceCard.curSelectCard == null ? 0.5f : 1);
            }

            if (isHide)
            {
                for (int i = 0; i < applyCards.Count; i++)
                {
                    choiceCards[i].GetComponent<SpriteRenderer>().color += Color.black * alpha;
                    choiceCards[i].GetComponentInChildren<TMP_Text>().color += Color.black * alpha;
                }

                if (TextTMP[0].color.a <= 0)
                {
                    break;
                }
            }
            else
            {
                if (TextTMP[0].color.a >= 1)
                {
                    break;
                }
            }

            yield return new WaitForEndOfFrame();
        }

        if (isHide)
        {
            gameObject.SetActive(false);
        }
        else
        {
            applyButton.SetButtonActive(false);
        }

        isFinishFade = true;
    }
}
