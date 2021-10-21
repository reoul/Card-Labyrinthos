using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EVENT_REWARD_TYPE { CARD, HP }

public class EventData
{
    public EVENT_REWARD_TYPE reward_type;
    public int index;

    public EventData(EVENT_REWARD_TYPE _reward_type, int _index)
    {
        reward_type = _reward_type;
        index = _index;
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
