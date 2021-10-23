using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] TMP_Text[] condition_TMP;
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
