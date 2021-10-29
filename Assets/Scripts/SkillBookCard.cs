using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBookCard : MonoBehaviour
{
    [SerializeField] GameObject frontCard;
    Card curSelectCard = null;
    public void SetCard(Card card)
    {
        frontCard.SetActive(true);
        frontCard.GetComponentInChildren<TMP_Text>().text = (card.final_Num + 1).ToString();
        if (curSelectCard == null)
        {
            card.SetColorAlpha(true);
            curSelectCard = card;
        }
        else
        {
            curSelectCard.SetColorAlpha(false);
            curSelectCard = card;
            curSelectCard.SetColorAlpha(true);
        }
    }

    public void HideCard()
    {
        frontCard.SetActive(false);
    }
}
