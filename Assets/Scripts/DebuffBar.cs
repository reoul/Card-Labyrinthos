using DG.Tweening;
using UnityEngine;

public class DebuffBar : MonoBehaviour
{
    private bool _isOpen = true;
    private bool _isMove;

    [SerializeField] private SpriteRenderer button;
    public Sprite open;
    public Sprite close;

    public void Open()
    {
        if (_isMove)
        {
            return;
        }

        _isMove = true;
        if (_isOpen)
        {
            Close();
            return;
        }

        SoundManager.Inst.Play(DEBUFFSOUND.OpenBar);
        transform.DOMove(new Vector3(6.94f, 3.65f, 0), 1).OnComplete(() =>
        {
            button.sprite = close;
            _isMove = false;
        });
        _isOpen = true;
    }

    public void Close()
    {
        SoundManager.Inst.Play(DEBUFFSOUND.CloseBar);
        transform.DOMove(new Vector3(10.89f, 3.65f, 0), 1).OnComplete(() =>
        {
            button.sprite = open;
            _isMove = false;
        });
        ;
        _isOpen = false;
    }
}
