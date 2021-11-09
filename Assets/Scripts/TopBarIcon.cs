using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBarIcon : MonoBehaviour
{
    public TOPBAR_TYPE type;
    bool onTopBarIcon = false;   //마우스가 필드 위에 있는지
    bool _isLock;
    public bool isLock
    {
        get { return _isLock; }
        private set { _isLock = value; }
    }

    void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        onTopBarIcon = true;
        switch (type)
        {
            case TOPBAR_TYPE.BAG:
            case TOPBAR_TYPE.SETTING:
            case TOPBAR_TYPE.SKILL:
                TopBar.Inst.OnMouseEnterIcon(type);
                break;
            default:
                break;
        }
    }
    void OnMouseExit()
    {
        onTopBarIcon = false;
        switch (type)
        {
            case TOPBAR_TYPE.BAG:
            case TOPBAR_TYPE.SETTING:
            case TOPBAR_TYPE.SKILL:
                TopBar.Inst.OnMouseExitIcon(type);
                break;
            default:
                break;
        }
    }

    private void OnMouseUp()
    {
        if (onTopBarIcon)
        {
            TopBar.Inst.Open(this);
        }
    }

    public void Lock()
    {
        isLock = true;
    }
    public void UnLock()
    {
        isLock = false;
    }

}
