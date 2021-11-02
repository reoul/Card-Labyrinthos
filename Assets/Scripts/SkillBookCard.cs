using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBookCard : MonoBehaviour
{
    public GameObject frontCard;
    public Card curSelectCard = null;
    [SerializeField] List<SkillBookCardButton> cardButtons;
    int _originNum;
    public int originNum
    {
        get
        {
            return _originNum;
        }
        set
        {
            _originNum = value;
            curNum = _originNum;
        }
    }
    int _curNum;
    public int curNum
    {
        get
        {
            return _curNum;
        }
        set
        {
            _curNum = Mathf.Clamp(value, 0, 5);
            this.transform.GetComponentInChildren<TMP_Text>().text = isQuestionMark ? "?" : (_curNum + 1).ToString();
            if (frontCard != null)
                frontCard.GetComponentInChildren<TMP_Text>().text = (_curNum + 1).ToString();
            if (!isHideButton)
            {
                if (cardButtons.Count == 2)
                {
                    if (_curNum == (limitNum == 0 ? 0 : Mathf.Clamp(_originNum - limitNum, 0, 5)))
                        cardButtons[1].gameObject.SetActive(false);
                    else if (_curNum == (limitNum == 0 ? 5 : Mathf.Clamp(_originNum + limitNum, 0, 5)))
                        cardButtons[0].gameObject.SetActive(false);
                    else
                    {
                        cardButtons[0].gameObject.SetActive(true);
                        cardButtons[1].gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < cardButtons.Count; i++)
                {
                    cardButtons[i].gameObject.SetActive(false);
                }
            }
            if ((originNum == curNum) && !isQuestionMark)
                SkillManager.Inst.ActivePage.applyButton.SetButtonActive(false);
            else
                SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);

            if (isApplyButtonOn)
                SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
        }
    }
    public int limitNum;    //+1을 하거나 특정 숫자만큼만 올리게 제한을 두는 변수
    public bool isHideButton;
    public bool isQuestionMark;
    public bool isApplyButtonOn;

    public void SetCard(Card card)
    {
        SkillManager.Inst.SetCard(this, card);
        for (int i = 0; i < cardButtons.Count; i++)
        {
            cardButtons[i].gameObject.SetActive(true);
        }
    }

    public void Up(int index = 1)
    {
        curNum += index;
    }
    public void Down(int index = 1)
    {
        curNum -= index;
    }

    public void HideCard()
    {
        //frontCard.GetComponent<SkillBookCard>().limitNum = 0;
        frontCard.SetActive(false);
    }

    public void SetColorAlpha(bool isHalf)
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0.5f : 1);
        this.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(isHalf ? 1 : 0, isHalf ? 1 : 0, isHalf ? 1 : 0, isHalf ? 0.5f : 1);     //숫자 텍스트
    }

    private void OnMouseOver()
    {
        if (curSelectCard != null)
            if (Input.GetMouseButtonUp(1))
            {
                HideCard();
                curSelectCard.SetColorAlpha(false);
                curSelectCard = null;
                SetColorAlpha(true);
                this.GetComponentInChildren<TMP_Text>().text = "+";
            }
    }
}
