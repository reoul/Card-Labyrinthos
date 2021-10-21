using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    bool onEvent = false;   //마우스가 필드 위에 있는지

    void OnMouseEnter()
    {
        onEvent = true;
    }
    void OnMouseExit()
    {
        onEvent = false;
    }

    public void MouseUp(EventData eventData)
    {
        if (onEvent)
        {

            EventManager.Inst.Choice(eventData);
        }
    }
}
