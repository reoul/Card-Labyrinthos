using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    bool onHelpButton = false;   //마우스가 필드 위에 있는지

    void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        onHelpButton = true;
    }
    void OnMouseExit()
    {
        onHelpButton = false;
    }

    private void OnMouseUp()
    {
        if (onHelpButton)
        {
            SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
