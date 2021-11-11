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

    bool flag = false;
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
                        cardButtons[0].gameObject.SetActive(isShowDownButton ? false : true);
                        cardButtons[1].gameObject.SetActive(isSecondMaxNum ? false : true);
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
            if (flag)
            {
                flag = false;
                return;
            }
            if (SkillManager.Inst.ActivePage.skill_type == SKILL_TYPE.SKILL5)
            {
                //if (SkillManager.Inst.ActivePage.applyCards[0].Equals(this))
                //{
                //    if (SkillManager.Inst.ActivePage.applyCards[0].gameObject.activeInHierarchy)
                //    {
                //        if (SkillManager.Inst.ActivePage.applyCards[1].gameObject.activeInHierarchy)
                //        {
                //            flag = true;
                //            SkillManager.Inst.ActivePage.applyCards[1].curNum = SkillManager.Inst.ActivePage.applyCards[0].curNum;
                //            SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
                //        }
                //    }
                //}
                //else
                //{
                //    if (SkillManager.Inst.ActivePage.applyCards[0].gameObject.activeInHierarchy)
                //    {
                //        flag = true;
                //        SkillManager.Inst.ActivePage.applyCards[1].curNum = SkillManager.Inst.ActivePage.applyCards[0].curNum;
                //        SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
                //    }
                //}
                if (SkillManager.Inst.ActivePage.applyCards[0].gameObject.activeInHierarchy && SkillManager.Inst.ActivePage.applyCards[1].gameObject.activeInHierarchy)
                {
                    flag = true;
                    SkillManager.Inst.ActivePage.applyCards[1].curNum = SkillManager.Inst.ActivePage.applyCards[0].curNum;
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
                }
                else
                {
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(false);
                }
            }
            if (SkillManager.Inst.ActivePage.skill_type != SKILL_TYPE.SKILL5)
            {
                if ((originNum == curNum) && !isQuestionMark)
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(false);
                else
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
            }

            if (isApplyButtonOn)
                SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
            //if (SkillManager.Inst.isUseSkill[SkillManager.Inst.ActivePageIndex])
            //{
            //    for (int i = 0; i < cardButtons.Count; i++)
            //    {
            //        cardButtons[i].gameObject.SetActive(false);
            //    }
            //    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(false);
            //}
        }
    }
    public int limitNum;    //+1을 하거나 특정 숫자만큼만 올리게 제한을 두는 변수
    public bool isHideButton;           //버튼을 둘다 숨겨야하는경우
    public bool isQuestionMark;         //카드 텍스트가 물음표여야하는경우
    public bool isApplyButtonOn;        //처음부터 적용 버튼을 켜야하는 경우
    public bool isShowDownButton;       //다운버튼만 보여줘야하는 경우
    public bool isSecondMaxNum;

    public void SetCard(Card card)
    {
        for (int i = 0; i < frontCard.GetComponent<SkillBookCard>().cardButtons.Count; i++)
        {
            frontCard.GetComponent<SkillBookCard>().cardButtons[i].gameObject.SetActive(true);
        }
        SkillManager.Inst.SetCard(this, card);
    }

    public void Up(int index = 1)
    {
        SoundManager.Inst.Play(SKILLBOOKSOUND.CARD_NUM_UP_DOWN);
        curNum += index;
        if (SkillManager.Inst.ActivePage.skill_type == SKILL_TYPE.SKILL2)
        {
            if (SkillManager.Inst.ActivePage.applyCards[0].Equals(this))
            {
                SkillManager.Inst.ActivePage.applyCards[1].curNum -= index;
            }
        }
    }
    public void Down(int index = 1)
    {
        SoundManager.Inst.Play(SKILLBOOKSOUND.CARD_NUM_UP_DOWN);
        curNum -= index;
        if (SkillManager.Inst.ActivePage.skill_type == SKILL_TYPE.SKILL2)
        {
            if (SkillManager.Inst.ActivePage.applyCards[0].Equals(this))
            {
                SkillManager.Inst.ActivePage.applyCards[1].curNum += index;
            }
        }
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

    public void Init()
    {
        HideCard();
        curSelectCard.SetColorAlpha(false);
        curSelectCard = null;
        SetColorAlpha(true);
        this.GetComponentInChildren<TMP_Text>().text = "+";
    }

    private void OnMouseOver()
    {
        if (curSelectCard != null)
            if (Input.GetMouseButtonUp(1))
            {
                if (SkillManager.Inst.ActivePage.skill_type == SKILL_TYPE.SKILL5 || SkillManager.Inst.ActivePage.skill_type == SKILL_TYPE.SKILL2)
                {
                    for (int i = 0; i < SkillManager.Inst.ActivePage.choiceCards.Count; i++)
                    {
                        SkillManager.Inst.ActivePage.choiceCards[i].Init();
                    }
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(false);
                }
                else
                {
                    Init();
                }
            }
    }
}
