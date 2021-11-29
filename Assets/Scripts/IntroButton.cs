using UnityEngine;

public enum IntroButtonType
{
    Start,
    Setting,
    Quit
}

public class IntroButton : MonoBehaviour
{
    private bool isButtonOn;
    private bool isClick;
    [SerializeField] private IntroButtonType type;

    private void OnMouseUp()
    {
        if (isButtonOn && !isClick)
        {
            Click();
        }
    }

    private void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
        isButtonOn = true;
    }

    private void OnMouseExit()
    {
        isButtonOn = false;
    }

    private void Click()
    {
        SoundManager.Inst.Play(EVENTSOUND.ChoiceButton);
        switch (type)
        {
            case IntroButtonType.Start:
                isClick = true;
                IntroManager.Inst.GameStart();
                break;
            case IntroButtonType.Setting:
                IntroManager.Inst.Setting();
                break;
            case IntroButtonType.Quit:
                IntroManager.Inst.GameQuit();
                break;
        }
    }
}
