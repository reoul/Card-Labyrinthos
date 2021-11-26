using DG.Tweening;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] TMP_Text notificationTmp;

    public void Show(string message)
    {
        this.notificationTmp.text = message;
        Sequence sequence = DOTween.Sequence().Append(this.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
            .AppendInterval(0.9f)
            .Append(this.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

    // Start is called before the first frame update
    void Start()
    {
        this.ScaleZero();
    }

    public void ScaleOne()
    {
        this.transform.localScale = Vector3.one;
    }

    public void ScaleZero()
    {
        this.transform.localScale = Vector3.zero;
    }
}
