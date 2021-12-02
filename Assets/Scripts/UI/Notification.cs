using DG.Tweening;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text notificationTmp;

    public void Show(string message)
    {
        notificationTmp.text = message;
        DOTween.Sequence().Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
               .AppendInterval(0.9f)
               .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

    private void Start()
    {
        ScaleZero();
    }

    private void ScaleZero()
    {
        transform.localScale = Vector3.zero;
    }
}
