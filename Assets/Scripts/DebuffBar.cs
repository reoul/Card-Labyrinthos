using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffBar : MonoBehaviour
{
    bool isOpen = true;
    bool isMove = false;

    [SerializeField] SpriteRenderer button;
    public Sprite open;
    public Sprite close;

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
        SoundManager.Inst.Play(DEBUFFSOUND.OPEN_BAR);
        this.transform.DOMove(new Vector3(6.94f, 3.65f, 0), 1).OnComplete(() =>
        {
            button.sprite = close;
            isMove = false;
        });
        isOpen = true;
    }

    public void Close()
    {
        SoundManager.Inst.Play(DEBUFFSOUND.CLOSE_BAR);
        this.transform.DOMove(new Vector3(10.89f, 3.65f, 0), 1).OnComplete(() =>
        {
            button.sprite = open;
            isMove = false;
        }); ;
        isOpen = false;
    }
}
