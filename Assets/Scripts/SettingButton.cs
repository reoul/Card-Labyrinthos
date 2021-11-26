using UnityEngine;

public enum SettingButtonType { BGM, SFX, RESET, QUIT }
public class SettingButton : MonoBehaviour
{
    [SerializeField] SettingButtonType type;
    bool isButtonOn;

    private void OnMouseUp()
    {
        if (this.isButtonOn) this.Click();
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
