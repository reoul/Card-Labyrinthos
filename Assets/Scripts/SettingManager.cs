using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Inst = null;

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

    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void SettingExit()
    {
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
