using System;
using TMPro;
using UnityEngine;

public class SkillBookCardButton : MonoBehaviour
{
    public SkillBookCard parent;

    [SerializeField] private bool onButton;

    private enum Type
    {
        Up,
        Down,
        Apply,
        Bookmark
    }

    public int index;
    public bool isActive = true;

    [SerializeField] private Type type;

    private int flag = 0;

    private void OnMouseUp()
    {
        if (onButton && isActive)
        {
            switch (type)
            {
                case Type.Up:
                    parent.Up();
                    break;
                case Type.Down:
                    parent.Down();
                    break;
                case Type.Apply:
                    if (!SkillManager.Inst.isUseSkill[SkillManager.Inst.ActivePageIndex])
                    {
                        SkillManager.Inst.ApplyCardAll();
                        SetButtonActive(false);
                    }

                    break;
                case Type.Bookmark:
                    SkillManager.Inst.SelectPage(index);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
        transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, isActive ? 1 : 0.5f); //숫자 텍스트
    }
}
