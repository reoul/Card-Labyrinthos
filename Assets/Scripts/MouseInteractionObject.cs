using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteractionObject : MonoBehaviour
{
    /// <summary>
    /// 마우스가 해당 오브젝트 위에 있는지
    /// </summary>
    protected bool OnMouse { get; set; }

    protected virtual void OnMouseEnter()
    {
        OnMouse = true;
    }
    
    protected virtual void OnMouseExit()
    {
        OnMouse = false;
    }
}
