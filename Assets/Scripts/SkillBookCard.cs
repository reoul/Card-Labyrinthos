using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBookCard : MonoBehaviour
{
    public GameObject frontCard;
    public Card curSelectCard;
    [SerializeField] private List<SkillBookCardButton> cardButtons;
    private int _originNum;

    public int OriginNum
    {
        get { return _originNum; }
        set
        {
            _originNum = value;
            CurNum = _originNum;
        }
    }

    private int _curNum;

    private bool flag;

    public int CurNum
    {
        get { return _curNum; }
        private set
        {
            _curNum = Mathf.Clamp(value, 0, 5);
            transform.GetComponentInChildren<TMP_Text>().text = isQuestionMark ? "?" : (_curNum + 1).ToString();
            if (frontCard != null)
            {
                frontCard.GetComponentInChildren<TMP_Text>().text = (_curNum + 1).ToString();
            }

            if (!isHideButton)
            {
                if (cardButtons.Count == 2)
                {
                    if (_curNum == (limitNum == 0 ? 0 : Mathf.Clamp(_originNum - limitNum, 0, 5)))
                    {
                        cardButtons[1].gameObject.SetActive(false);
                    }
                    else if (_curNum == (limitNum == 0 ? 5 : Mathf.Clamp(_originNum + limitNum, 0, 5)))
                    {
                        cardButtons[0].gameObject.SetActive(false);
                    }
                    else
                    {
                        cardButtons[0].gameObject.SetActive(!isShowDownButton);
                        cardButtons[1].gameObject.SetActive(!isSecondMaxNum);
                    }
                }
            }
            else
            {
                foreach (SkillBookCardButton cardUpDownBtn in cardButtons)
                {
                    cardUpDownBtn.gameObject.SetActive(false);
                }
            }

            if (flag)
            {
                flag = false;
                return;
            }

            if (SkillManager.Inst.ActivePage.skill_type == SKILL_TYPE.Skill5)
            {
                if (SkillManager.Inst.ActivePage.applyCards[0].gameObject.activeInHierarchy &&
                    SkillManager.Inst.ActivePage.applyCards[1].gameObject.activeInHierarchy)
                {
                    flag = true;
                    SkillManager.Inst.ActivePage.applyCards[1].CurNum =
                        SkillManager.Inst.ActivePage.applyCards[0].CurNum;
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
                }
                else
                {
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(false);
                }
            }

            if (SkillManager.Inst.ActivePage.skill_type != SKILL_TYPE.Skill5)
            {
                if ((OriginNum == CurNum) && !isQuestionMark)
                {
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(false);
                }
                else
                {
                    SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
                }
            }

            if (isApplyButtonOn)
            {
                SkillManager.Inst.ActivePage.applyButton.SetButtonActive(true);
            }
        }
    }

    public int limitNum; //+1을 하거나 특정 숫자만큼만 올리게 제한을 두는 변수
    public bool isHideButton; //버튼을 둘다 숨겨야하는경우
    public bool isQuestionMark; //카드 텍스트가 물음표여야하는경우
    public bool isApplyButtonOn; //처음부터 적용 버튼을 켜야하는 경우
    public bool isShowDownButton; //다운버튼만 보여줘야하는 경우
    public bool isSecondMaxNum;

    public void SetCard(Card card)
    {
        foreach (var cardUpDownBtn in frontCard.GetComponent<SkillBookCard>().cardButtons)
        {
            cardUpDownBtn.gameObject.SetActive(true);
        }

        SkillManager.Inst.SetCard(this, card);
    }

    public void Up(int index = 1)
    {
        SoundManager.Inst.Play(SKILLBOOKSOUND.CardNumUpDown);
        CurNum += index;

        if (SkillManager.Inst.ActivePage.skill_type != SKILL_TYPE.Skill2)
        {
            return;
        }

        if (SkillManager.Inst.ActivePage.applyCards[0].Equals(this))
        {
            SkillManager.Inst.ActivePage.applyCards[1].CurNum -= index;
        }
    }

    public void Down(int index = 1)
    {
        SoundManager.Inst.Play(SKILLBOOKSOUND.CardNumUpDown);
        CurNum -= index;

        if (SkillManager.Inst.ActivePage.skill_type != SKILL_TYPE.Skill2)
        {
            return;
        }

        if (SkillManager.Inst.ActivePage.applyCards[0].Equals(this))
        {
            SkillManager.Inst.ActivePage.applyCards[1].CurNum += index;
        }
    }

    public void HideCard()
    {
        frontCard.SetActive(false);
    }

    public void SetColorAlpha(bool isHalf)
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0.5f : 1);
        transform.GetChild(0).GetComponent<TMP_Text>().color =
            new Color(isHalf ? 1 : 0, isHalf ? 1 : 0, isHalf ? 1 : 0, isHalf ? 0.5f : 1); //숫자 텍스트
    }

    private void Init()
    {
        HideCard();
        curSelectCard?.SetColorAlpha(false);
        curSelectCard = null;
        SetColorAlpha(true);
        GetComponentInChildren<TMP_Text>().text = "+";
    }

    private void OnMouseOver()
    {
        if (curSelectCard == null)
        {
            return;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (SkillManager.Inst.ActivePage.skill_type == SKILL_TYPE.Skill5 ||
                SkillManager.Inst.ActivePage.skill_type == SKILL_TYPE.Skill2)
            {
                foreach (var choiceCard in SkillManager.Inst.ActivePage.choiceCards)
                {
                    choiceCard.Init();
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
