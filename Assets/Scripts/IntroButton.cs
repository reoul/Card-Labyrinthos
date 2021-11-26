using UnityEngine;

public enum IntroButtonType { START, SETTING, QUIT }
public class IntroButton : MonoBehaviour
{
    bool isButtonOn;
    bool isClick;
    [SerializeField] IntroButtonType type;

    private void OnMouseUp()
    {
        if (this.isButtonOn && !this.isClick) this.Click();
    }

    private void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        this.isButtonOn = true;
    }

    private void OnMouseExit()
    {
        this.isButtonOn = false;
    }

    void Click()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
        switch (this.type)
        {
            case IntroButtonType.START:
                this.isClick = true;
                IntroManager.Inst.GameStart();
                break;
            case IntroButtonType.SETTING:
                IntroManager.Inst.Setting();
                break;
            case IntroButtonType.QUIT:
                IntroManager.Inst.GameQuit();
                break;
        }
    }
}
