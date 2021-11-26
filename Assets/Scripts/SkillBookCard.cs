using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBookCard : MonoBehaviour
{
    public GameObject frontCard;
    public Card curSelectCard;
    [SerializeField] List<SkillBookCardButton> cardButtons;
    int _originNum;
    public int originNum
    {
        get
        {
            return this._originNum;
        }
        set
        {
            this._originNum = value;
            this.curNum = this._originNum;
        }
    }
    int _curNum;

    bool flag;
    public int curNum
    {
        get
        {
            return this._curNum;
        }
        set
        {
            this._curNum = Mathf.Clamp(value, 0, 5);
            this.transform.GetComponentInChildren<TMP_Text>().text = this.isQuestionMark ? "?" : (this._curNum + 1).ToString();
            if (this.frontCard != null) this.frontCard.GetComponentInChildren<TMP_Text>().text = (this._curNum + 1).ToString();
            if (!this.isHideButton)
            {
                if (this.cardButtons.Count == 2)
                {
                    if (this._curNum == (this.limitNum == 0 ? 0 : Mathf.Clamp(this._originNum - this.limitNum, 0, 5)))
                        this.cardButtons[1].gameObject.SetActive(false);
                    else if (this._curNum == (this.limitNum == 0 ? 5 : Mathf.Clamp(this._originNum + this.limitNum, 0, 5)))
                        this.cardButtons[0].gameObject.SetActive(false);
                    else
                    {
                        this.cardButtons[0].gameObject.SetActive(this.isShowDownButton ? false : true);
                        this.cardButtons[1].gameObject.SetActive(this.isSecondMaxNum ? false : true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.cardButtons.Count; i++)
                {
                    this.cardButtons[i].gameObject.SetActive(false);
                }
            }
            if (this.flag)
            {
                this.flag = false;
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
                    this.flag = true;
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
                if ((this.originNum == this.curNum) && !this.isQuestionMark)
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(false);
                else
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
            }

            if (this.isApplyButtonOn)
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
        for (int i = 0; i < this.frontCard.GetComponent<SkillBookCard>().cardButtons.Count; i++)
        {
            this.frontCard.GetComponent<SkillBookCard>().cardButtons[i].gameObject.SetActive(true);
        }
        SkillManager.Inst.SetCard(this, card);
    }

    public void Up(int index = 1)
    {
        SoundManager.Inst.Play(SKILLBOOKSOUND.CARD_NUM_UP_DOWN);
        this.curNum += index;
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
        this.curNum -= index;
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
        this.frontCard.SetActive(false);
    }

    public void SetColorAlpha(bool isHalf)
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0.5f : 1);
        this.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(isHalf ? 1 : 0, isHalf ? 1 : 0, isHalf ? 1 : 0, isHalf ? 0.5f : 1);     //숫자 텍스트
    }

    public void Init()
    {
        this.HideCard();
        this.curSelectCard?.SetColorAlpha(false);
        this.curSelectCard = null;
        this.SetColorAlpha(true);
        this.GetComponentInChildren<TMP_Text>().text = "+";
    }

    private void OnMouseOver()
    {
        if (this.curSelectCard != null)
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
                    this.Init();
                }
            }
    }
}
