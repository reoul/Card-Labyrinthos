using UnityEngine;

public class HelpButton : MonoBehaviour
{
    bool onHelpButton;   //마우스가 필드 위에 있는지

    void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        this.onHelpButton = true;
    }
    void OnMouseExit()
    {
        this.onHelpButton = false;
    }

    private void OnMouseUp()
    {
        if (this.onHelpButton)
        {
            SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
