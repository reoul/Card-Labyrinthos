using TMPro;
using UnityEngine;

public class SkillBookCardButton : MonoBehaviour
{
    public SkillBookCard parent;

    [SerializeField] private bool onButton;

    private enum TYPE { UP, DOWN, APPLY, BOOKMARK }
    public int index;
    public bool isActive = true;

    [SerializeField] private TYPE type;

    private int flag = 0;

    private void OnMouseUp()
    {
        if (onButton && isActive)
        {
            switch (type)
            {
                case TYPE.UP:
                    parent.Up();
                    break;
                case TYPE.DOWN:
                    parent.Down();
                    break;
                case TYPE.APPLY:
                    if (!SkillManager.Inst.isUseSkill[SkillManager.Inst.ActivePageIndex])
                    {
                        switch (SkillManager.Inst.ActivePage.skill_type)
                        {
                            case SKILL_TYPE.SKILL1:
                                break;
                            case SKILL_TYPE.SKILL2:
                                break;
                            case SKILL_TYPE.SKILL3:
                                break;
                            case SKILL_TYPE.SKILL4:
                                break;
                            case SKILL_TYPE.SKILL5:
                                break;
                            case SKILL_TYPE.SKILL6:
                                break;
                        }
                        SkillManager.Inst.ApplyCardAll();
                        SetButtonActive(false);
                    }
                    break;
                case TYPE.BOOKMARK:
                    SkillManager.Inst.SelectPage(index);
                    break;
            }
        }
    }

    private void OnMouseEnter()
    {
        onButton = true;
    }

    private void OnMouseExit()
    {
        onButton = false;
    }

    public void SetButtonActive(bool isActive)
    {
        this.isActive = isActive;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isActive ? 1 : 0.5f);
        transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, isActive ? 1 : 0.5f);     //숫자 텍스트
    }
}
