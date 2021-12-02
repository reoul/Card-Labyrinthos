using System;
using UnityEngine;

public class TopBarIcon : MouseInteractionObject
{
    public TOPBAR_TYPE type;
    
    public bool IsLock { get; private set; }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
        switch (type)
        {
            case TOPBAR_TYPE.Bag:
            case TOPBAR_TYPE.Setting:
            case TOPBAR_TYPE.Skill:
                TopBarManager.Inst.OnMouseEnterIcon(type);
                break;
            case TOPBAR_TYPE.Hp:
                break;
            case TOPBAR_TYPE.Question:
                break;
            case TOPBAR_TYPE.CardPiece:
                break;
            case TOPBAR_TYPE.SceneName:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();
        switch (type)
        {
            case TOPBAR_TYPE.Bag:
            case TOPBAR_TYPE.Setting:
            case TOPBAR_TYPE.Skill:
                TopBarManager.Inst.OnMouseExitIcon(type);
                break;
            case TOPBAR_TYPE.Hp:
                break;
            case TOPBAR_TYPE.Question:
                break;
            case TOPBAR_TYPE.CardPiece:
                break;
            case TOPBAR_TYPE.SceneName:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseUp()
    {
        if (OnMouse)
        {
            TopBarManager.Inst.Open(this);
        }
    }
}
