using TMPro;
using UnityEngine;

public class SkillBookCardButton : MonoBehaviour
{
    public SkillBookCard parent;

    [SerializeField] bool onButton;

    enum TYPE { UP, DOWN, APPLY, BOOKMARK }
    public int index;
    public bool isActive = true;

    [SerializeField] TYPE type;

    int flag = 0;

    private void OnMouseUp()
    {
        if (this.onButton && this.isActive)
            switch (this.type)
            {
                case TYPE.UP:
                    this.parent.Up();
                    break;
                case TYPE.DOWN:
                    this.parent.Down();
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
                        this.SetButtonActive(false);
                    }
                    break;
                case TYPE.BOOKMARK:
                    SkillManager.Inst.SelectPage(this.index);
                    break;
            }
    }

    private void OnMouseEnter()
    {
        this.onButton = true;
    }

    private void OnMouseExit()
    {
        this.onButton = false;
    }

    public void SetButtonActive(bool isActive)
    {
        this.isActive = isActive;
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isActive ? 1 : 0.5f);
        this.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, isActive ? 1 : 0.5f);     //숫자 텍스트
    }
}
