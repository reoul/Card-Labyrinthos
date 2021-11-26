using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public void Start()
    {
        MoveDown();
    }

    private void MoveUp()
    {
        transform.DOLocalMove(transform.localPosition + transform.up * 0.5f, 0.75f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            MoveDown();
        });
    }

    private void MoveDown()
    {
        transform.DOLocalMove(transform.localPosition + -transform.up * 0.5f, 0.75f).SetEase(Ease.InBack).OnComplete(() =>
        {
            MoveUp();
        });
    }

    public void ArrowDestory()
    {
        transform.DOPause();
        Destroy(gameObject);
    }
}
