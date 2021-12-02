using System;
using UnityEngine;

public enum IntroButtonType
{
    Start,
    Setting,
    Quit
}

public class IntroButton : MouseInteractionObject
{
    private bool isClick;
    [SerializeField] private IntroButtonType type;

    private void OnMouseUp()
    {
        if (OnMouse && !isClick)
        {
            Click();
        }
    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
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
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
