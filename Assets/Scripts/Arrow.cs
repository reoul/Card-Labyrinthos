using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public void Start()
    {
        this.MoveDown();
    }

    private void MoveUp()
    {
        this.transform.DOLocalMove(this.transform.localPosition + this.transform.up * 0.5f, 0.75f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            this.MoveDown();
        });
    }

    private void MoveDown()
    {
        this.transform.DOLocalMove(this.transform.localPosition + -this.transform.up * 0.5f, 0.75f).SetEase(Ease.InBack).OnComplete(() =>
        {
            this.MoveUp();
        });
    }

    public void ArrowDestory()
    {
        this.transform.DOPause();
        Destroy(this.gameObject);
    }
}
