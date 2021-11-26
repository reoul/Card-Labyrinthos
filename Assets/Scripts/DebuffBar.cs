using DG.Tweening;
using UnityEngine;

public class DebuffBar : MonoBehaviour
{
    bool isOpen = true;
    bool isMove;

    [SerializeField] SpriteRenderer button;
    public Sprite open;
    public Sprite close;

    public void Open()
    {
        if (this.isMove)
            return;
        this.isMove = true;
        if (this.isOpen)
        {
            this.Close();
            return;
        }
        SoundManager.Inst.Play(DEBUFFSOUND.OPEN_BAR);
        this.transform.DOMove(new Vector3(6.94f, 3.65f, 0), 1).OnComplete(() =>
        {
            this.button.sprite = this.close;
            this.isMove = false;
        });
        this.isOpen = true;
    }

    public void Close()
    {
        SoundManager.Inst.Play(DEBUFFSOUND.CLOSE_BAR);
        this.transform.DOMove(new Vector3(10.89f, 3.65f, 0), 1).OnComplete(() =>
        {
            this.button.sprite = this.open;
            this.isMove = false;
        }); ;
        this.isOpen = false;
    }
}
