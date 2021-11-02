using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBarIcon : MonoBehaviour
{
    public TOPBAR_TYPE type;
    bool onTopBarIcon = false;   //마우스가 필드 위에 있는지

    void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        onTopBarIcon = true;
    }
    void OnMouseExit()
    {
        onTopBarIcon = false;
    }

    private void OnMouseUp()
    {
        if (onTopBarIcon)
        {
            TopBar.Inst.Click(this);
        }
    }
}
