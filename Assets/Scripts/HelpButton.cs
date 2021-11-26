using UnityEngine;

public class HelpButton : MonoBehaviour
{
    private bool onHelpButton;   //마우스가 필드 위에 있는지

    private void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        onHelpButton = true;
    }

    private void OnMouseExit()
    {
        onHelpButton = false;
    }

    private void OnMouseUp()
    {
        if (onHelpButton)
        {
            SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
