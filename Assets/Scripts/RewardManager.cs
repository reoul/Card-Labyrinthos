using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Inst = null;

    public GameObject rewardWindowPrefab;
    public GameObject rewardPrefab;

    public List<GameObject> rewards;

    public bool isGetAllReward = false;

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

    private void Update()
    {

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

    public IEnumerator RewardCorutine()
    {
        isGetAllReward = false;
        StartCoroutine(CheckGetAllReward());
        while (true)
        {
            //if (isGetAllReward)
            if (getReward)
            {
                isGetAllReward = false;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator CheckGetAllReward()
    {
        while (true)
        {
            if (rewards.Count == 0)
            {
                isGetAllReward = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
