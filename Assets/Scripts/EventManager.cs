using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EVENT_REWARD_TYPE { CARD, CARD_PIECE, HP, DRAW }

public class EventData
{
    public REWARD_KIND reward_kind;
    public int first_reward_probability;
    public EVENT_REWARD_TYPE reward_type1_1;
    public int index1_1;
    public EVENT_REWARD_TYPE reward_type1_2;
    public int index1_2;
    public EVENT_REWARD_TYPE reward_type2;
    public int index2;

    public EventData(REWARD_KIND reward_kind, int first_reward_probability, EVENT_REWARD_TYPE reward_type1_1, int index1_1, EVENT_REWARD_TYPE reward_type1_2, int index1_2, EVENT_REWARD_TYPE reward_type2, int index2)
    {
        this.reward_kind = reward_kind;
        this.first_reward_probability = first_reward_probability;
        this.reward_type1_1 = reward_type1_1;
        this.index1_1 = index1_1;
        this.reward_type1_2 = reward_type1_2;
        this.index1_2 = index1_2;
        this.reward_type2 = reward_type2;
        this.index2 = index2;
    }
}

public class EventManager : MonoBehaviour
{
    public static EventManager Inst;

    public List<Event> events;

    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Choice(EventData eventData)
    {

    }

    void FindEvents()
    {
        Event[] events = GameObject.Find("Event").GetComponentsInChildren<Event>();
        for (int i = 0; i < events.Length; i++)
        {
            events[i].gameObject.SetActive(false);
            this.events.Add(events[i]);
        }
    }

    public IEnumerator RandomEventCorutine()
    {
        FindEvents();
        int rand = Random.Range(0, events.Count);
        events[rand].gameObject.SetActive(true);
        yield return null;
    }
}
