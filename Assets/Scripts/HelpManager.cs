using System.Collections.Generic;
using UnityEngine;

public enum HELP_TYPE { BATTLE, MAP, DEBUFFE, BAG, SKILL, SHOP, EVENT }
public class HelpManager : MonoBehaviour
{
    public static HelpManager Inst;

    bool[] isShowOnce;
    [SerializeField] List<GameObject> helps;

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

        this.isShowOnce = new bool[7];
    }

    public void ShowHelp(HELP_TYPE type)
    {
        if (!this.isShowOnce[(int)type])
        {
            SoundManager.Inst.Play(REWARDSOUND.SHOW_REWARD_WINDOW);
            this.helps[(int)type].SetActive(true);
            this.isShowOnce[(int)type] = true;
        }
    }
}
