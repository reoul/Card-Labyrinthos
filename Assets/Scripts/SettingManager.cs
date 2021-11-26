using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Inst;

    [SerializeField] GameObject settingWindow;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Open()
    {
        if (this.settingWindow.activeInHierarchy)
        {
            this.Close();
            return;
        }

        this.settingWindow.SetActive(true);
    }

    public void Close()
    {
        this.settingWindow.SetActive(false);
    }

    public void GameReset()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
        ResetManager.Inst.ResetGame();
    }

    public void GameQuit()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
        Application.Quit();
    }

    public void SettingExit()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
        this.Close();
    }

    public void UpdateBGMVolume()
    {
        SoundManager.Inst.SetBGMVolume(this.bgmSlider.value);
    }

    public void UpdateSFXVolume()
    {
        SoundManager.Inst.SetSFXVolume(this.sfxSlider.value);
    }
}
