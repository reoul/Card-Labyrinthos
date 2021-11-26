using UnityEngine;

public enum SettingButtonType { BGM, SFX, RESET, QUIT }
public class SettingButton : MonoBehaviour
{
    [SerializeField] private SettingButtonType type;
    private bool isButtonOn;

    private void OnMouseUp()
    {
        if (isButtonOn)
        {
            Click();
        }
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

    private void Click()
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
