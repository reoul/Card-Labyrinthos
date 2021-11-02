using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBookPage : MonoBehaviour
{
    public List<SkillBookCard> choiceCards;
    public List<SkillBookCard> applyCards;

    [SerializeField] List<TMP_Text> TextTMP;
    [SerializeField] List<SpriteRenderer> renderers;
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
        StartCoroutine(ColorAlphaCorutine(false));
    }
    public void Hide()
    {
        StartCoroutine(ColorAlphaCorutine(true));
    }

    IEnumerator ColorAlphaCorutine(bool isHide)
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
    }

}
