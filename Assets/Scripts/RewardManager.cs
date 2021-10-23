using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Inst = null;

    public GameObject rewardWindowPrefab;
    public GameObject rewardPrefab;

    public List<GameObject> rewards;

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

    public bool getReward;

    public void ShowRewardWindow()
    {
    }

    public void AddReward(EVENT_REWARD_TYPE reward_type, int index)
    {
        GameObject reward = Instantiate(rewardPrefab);
        rewards.Add(reward);
    }
}
