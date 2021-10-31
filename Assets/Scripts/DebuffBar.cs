using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffBar : MonoBehaviour
{
    bool isOpen = true;
    bool isMove = false;

    public void Open()
    {
        if (isMove)
            return;
        isMove = true;
        if (isOpen)
        {
            Close();
            return;
        }
        this.transform.DOMove(new Vector3(6.94f, 3.65f, 0), 1).OnComplete(() =>
        {
            isMove = false;
        });
        isOpen = true;
    }

    public void Close()
    {
        this.transform.DOMove(new Vector3(10.89f, 3.65f, 0), 1).OnComplete(() =>
        {
            isMove = false;
        }); ;
        isOpen = false;
    }
}
