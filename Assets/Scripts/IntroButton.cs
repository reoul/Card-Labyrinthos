using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IntroButtonType { START, SETTING, QUIT }
public class IntroButton : MonoBehaviour
{
    bool isButtonOn = false;
    bool isClick = false;
    [SerializeField] IntroButtonType type;

    private void OnMouseUp()
    {
        if (isButtonOn && !isClick)
            Click();
    }

    private void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        isButtonOn = true;
    }

    private void OnMouseExit()
    {
        isButtonOn = false;
    }

    void Click()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
        switch (type)
        {
            case IntroButtonType.START:
                isClick = true;
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
