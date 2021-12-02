using UnityEngine;
using UnityEngine.UI;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private GameObject settingWindow;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);
    }

    public void Open()
    {
        if (settingWindow.activeInHierarchy)
        {
            Close();
            return;
        }

        settingWindow.SetActive(true);
    }

    public void Close()
    {
        settingWindow.SetActive(false);
    }

    public void GameReset()
    {
        SoundManager.Inst.Play(EVENTSOUND.ChoiceButton);
        ResetManager.Inst.ResetGame();
    }

    public void GameQuit()
    {
        SoundManager.Inst.Play(EVENTSOUND.ChoiceButton);
        Application.Quit();
    }

    public void SettingExit()
    {
        SoundManager.Inst.Play(EVENTSOUND.ChoiceButton);
        Close();
    }

    public void UpdateBGMVolume()
    {
        SoundManager.Inst.SetBGMVolume(bgmSlider.value);
    }

    public void UpdateSFXVolume()
    {
        SoundManager.Inst.SetSfxVolume(sfxSlider.value);
    }
}
