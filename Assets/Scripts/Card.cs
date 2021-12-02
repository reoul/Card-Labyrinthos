using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : MouseInteractionObject
{
    private int OriginalNum { get; set; }
    private int _finalNum;

    public int FinalNum
    {
        get => _finalNum;
        private set
        {
            _finalNum = Mathf.Clamp(value, 0, 5);
            UpdateNumTMP();
        }
    }

    public TMP_Text num_TMP;

    public PRS originPRS;
    public Transform Parent => transform.parent;
    public bool isFinish;

    private bool IsLock { get; set; }

    public void Setup(int num)
    {
        OriginalNum = num;
        FinalNum = num;
        UpdateNumTMP();
    }

    public void RevertOriginNum()
    {
        FinalNum = OriginalNum;
    }

    private void UpdateNumTMP()
    {
        num_TMP.text = (FinalNum + 1).ToString();
    }

    public void MoveTransform(PRS prs, bool useDoTween, float doTweenTime = 0)
    {
        if (useDoTween)
        {
            Parent.DOMove(prs.Pos, doTweenTime);
            Parent.DORotateQuaternion(prs.Rot, doTweenTime);
            Parent.DOScale(prs.Scale, doTweenTime);
        }
        else
        {
            Parent.position = prs.Pos;
            Parent.rotation = prs.Rot;
            Parent.localScale = prs.Scale;
        }
    }

    public void FinishCard()
    {
        Parent.localScale = Vector3.one * 0.1f;
        SetActiveChildObj(false);
    }

    public void SetActiveChildObj(bool isActive)
    {
        Parent.GetChild(1).gameObject.SetActive(isActive);
        Parent.GetChild(2).gameObject.SetActive(isActive);
    }

    protected override void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
    }

    private void OnMouseOver()
    {
        CardManager.Inst.CardMouseOver(this);
    }

    protected override void OnMouseExit()
    {
        CardManager.Inst.CardMouseExit(this);
    }

    private void OnMouseDown()
    {
        if (!FadeManager.Inst.isActiveFade && !isFinish && !IsLock)
        {
            CardManager.Inst.CardMouseDown();
        }
    }

    private void OnMouseUp()
    {
        if (!isFinish && !IsLock)
        {
            CardManager.Inst.CardMouseUp();
        }
    }

    public void Use(GameObject target = null)
    {
        if (!(target is null) && target.CompareTag("Player"))
        {
            Player.Inst.AddShield(FinalNum + 1);
        }
    }

    public void FinishScene()
    {
        isFinish = true;
        MoveTransform(originPRS, false);
        MoveTransform(new PRS(originPRS.Pos - Vector3.up * 3, originPRS.Rot, originPRS.Scale), true,
            0.3f);
    }

    public void SetFinalNum(int index)
    {
        FinalNum = index;
    }

    public void SetColorAlpha(bool isHalf)
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0 : 1);
        Parent.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0.5f : 1); //카드 앞면
        Parent.GetChild(2).GetComponent<TMP_Text>().color = new Color(0, 0, 0, isHalf ? 0.5f : 1); //숫자 텍스트
    }

    public void Lock()
    {
        IsLock = true;
    }

    public void UnLock()
    {
        IsLock = false;
    }

    public void SetOrderLayer(int index)
    {
        GetComponent<SpriteRenderer>().sortingOrder = index;
        Parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = index + 1;
        Parent.GetChild(2).GetComponent<Renderer>().sortingOrder = index + 2;
    }
}
