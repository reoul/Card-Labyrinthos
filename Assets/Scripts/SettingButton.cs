using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SettingButtonType { BGM, SFX, RESET, QUIT }
public class SettingButton : MonoBehaviour
{
    [SerializeField] SettingButtonType type;
    bool isButtonOn = false;

    private void OnMouseUp()
    {
        if (isButtonOn)
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
            case SettingButtonType.BGM:
                break;
            case SettingButtonType.SFX:
                break;
            case SettingButtonType.RESET:
                break;
            case SettingButtonType.QUIT:
                Application.Quit();
                break;
        }
    }

}
