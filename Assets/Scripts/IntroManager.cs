using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public static IntroManager Inst;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.INTRO);
    }

    public void GameStart()
    {
        MapManager.Inst.LoadTutorialScene();
    }

    public void Setting()
    {
        SettingManager.Inst.Open();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
