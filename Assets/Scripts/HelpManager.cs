using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HELP_TYPE { BATTLE, MAP, DEBUFFE, BAG, SKILL, SHOP, EVENT }
public class HelpManager : MonoBehaviour
{
    public static HelpManager Inst = null;

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
        isShowOnce = new bool[7];
    }

    public void ShowHelp(HELP_TYPE type)
    {
        if (!isShowOnce[(int)type])
        {
            SoundManager.Inst.Play(REWARDSOUND.SHOW_REWARD_WINDOW);
            helps[(int)type].SetActive(true);
            isShowOnce[(int)type] = true;
        }
    }
}
