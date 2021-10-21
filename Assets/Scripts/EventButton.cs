using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButton : MonoBehaviour
{
    public EVENT_REWARD_TYPE reward_type;
    public int index;
    public EventData eventData { get { return new EventData(reward_type, index); } }

    bool onEvent = false;   //마우스가 필드 위에 있는지
    void OnMouseUp()
    {
        if (onEvent)
        {
            this.transform.parent.GetComponent<Event>().MouseUp(eventData);
        }
    }
    void OnMouseEnter()
    {
        onEvent = true;
    }
    void OnMouseExit()
    {
        onEvent = false;
    }
}
