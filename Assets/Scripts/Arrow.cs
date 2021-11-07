using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public void Start()
    {
        MoveB();
    }
    public void MoveA()
    {
        this.transform.DOMove(this.transform.position + transform.up * 0.5f, 0.75f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            MoveB();
        });
    }

    public void MoveB()
    {
        this.transform.DOMove(this.transform.position + -transform.up * 0.5f, 0.75f).SetEase(Ease.InBack).OnComplete(() =>
        {
            MoveA();
        });
    }

    public void ArrowDestory()
    {
        this.transform.DOPause();
        Destroy(this.gameObject);
    }
}
