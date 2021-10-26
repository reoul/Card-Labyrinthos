using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffIcon : MonoBehaviour
{
    bool onDebuffIcon = false;   //마우스가 필드 위에 있는지

    void OnMouseEnter()
    {
        onDebuffIcon = true;
    }
    void OnMouseExit()
    {
        onDebuffIcon = false;
    }

    private void OnMouseUp()
    {
        if (onDebuffIcon)
        {
            //TopBar.Inst.Click(this);
        }
    }
}
