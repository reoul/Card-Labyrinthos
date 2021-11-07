using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IntroButtonType { START, SETTING, QUIT }
public class IntroButton : MonoBehaviour
{
    bool isButtonOn = false;
    [SerializeField] IntroButtonType type;

    private void OnMouseUp()
    {
        if (isButtonOn)
            Click();
    }

    private void OnMouseEnter()
    {
        isButtonOn = true;
    }

    private void OnMouseExit()
    {
        isButtonOn = false;
    }

    void Click()
    {
        switch (type)
        {
            case IntroButtonType.START:
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
