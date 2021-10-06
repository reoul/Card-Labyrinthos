using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] TMP_Text notificationTmp;

    public void Show(string message)
    {
        notificationTmp.text = message;
        Sequence sequence = DOTween.Sequence().Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
            .AppendInterval(0.9f)
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

    // Start is called before the first frame update
    void Start()
    {
        ScaleZero();
    }

    public void ScaleOne()
    {
        transform.localScale = Vector3.one;
    }

    public void ScaleZero()
    {
        transform.localScale = Vector3.zero;
    }
}
