using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Inst;

    [SerializeField] private GameObject settingWindow;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        Close();
    }

    public void UpdateBGMVolume()
    {
        SoundManager.Inst.SetBGMVolume(bgmSlider.value);
    }

    public void UpdateSFXVolume()
    {
        SoundManager.Inst.SetSFXVolume(sfxSlider.value);
    }
}
