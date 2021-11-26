using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int original_Num { get; private set; }
    private int _final_Num;

    public int final_Num
    {
        get => _final_Num;
        private set
        {
            _final_Num = Mathf.Clamp(value, 0, 5);
            UpdateNumTMP();
        }
    }

    public TMP_Text num_TMP;

    public PRS originPRS;
    public Transform parent => transform.parent;
    public bool isFinish;
    private bool _isLock;

    public bool isLock
    {
        get => _isLock;
        private set => _isLock = value;
    }

    public void Setup(int num)
    {
        original_Num = num;
        final_Num = num;
        UpdateNumTMP();
    }

    public void RevertOriginNum()
    {
        final_Num = original_Num;
    }

    private void UpdateNumTMP()
    {
        num_TMP.text = (final_Num + 1).ToString();
    }

    public void MoveTransform(PRS prs, bool useDoTween, float doTweenTime = 0)
    {
        if (useDoTween)
        {
            parent.DOMove(prs.pos, doTweenTime);
            parent.DORotateQuaternion(prs.rot, doTweenTime);
            parent.DOScale(prs.scale, doTweenTime);
        }
        else
        {
            parent.position = prs.pos;
            parent.rotation = prs.rot;
            parent.localScale = prs.scale;
        }
    }

    public void FinishCard()
    {
        parent.localScale = Vector3.one * 0.1f;
        SetActiveChildObj(false);
    }

    public void SetActiveChildObj(bool isActive)
    {
        parent.GetChild(1).gameObject.SetActive(isActive);
        parent.GetChild(2).gameObject.SetActive(isActive);
    }

    private void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
    }

    private void OnMouseOver()
    {
        CardManager.Inst.CardMouseOver(this);
    }

    private void OnMouseExit()
    {
        CardManager.Inst.CardMouseExit(this);
    }

    private void OnMouseDown()
    {
        if (!FadeManager.Inst.isActiveFade && !isFinish && !isLock)
        {
            CardManager.Inst.CardMouseDown();
        }
    }

    private void OnMouseUp()
    {
        if (!isFinish && !isLock)
        {
            CardManager.Inst.CardMouseUp();
        }
    }

    public void Use(GameObject obj = null)
    {
        if (obj.CompareTag("Player"))
        {
            Player.Inst.Sheld(final_Num + 1);
        }
        else
        {
            Player.Inst.Attack();
        }
    }

    public void FinishScene()
    {
        isFinish = true;
        MoveTransform(originPRS, false);
        MoveTransform(new PRS(originPRS.pos - Vector3.up * 3, originPRS.rot, originPRS.scale), true,
            0.3f);
    }

    public void AddNum(int index)
    {
        final_Num += index;
    }

    public void SetFinalNum(int index)
    {
        final_Num = index;
    }

    public void SetColorAlpha(bool isHalf)
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0 : 1);
        parent.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0.5f : 1); //카드 앞면
        parent.GetChild(2).GetComponent<TMP_Text>().color = new Color(0, 0, 0, isHalf ? 0.5f : 1); //숫자 텍스트
    }

    public void Lock()
    {
        isLock = true;
    }

    public void UnLock()
    {
        isLock = false;
    }

    public void SetOrderLayer(int index)
    {
        GetComponent<SpriteRenderer>().sortingOrder = index;
        parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = index + 1;
        parent.GetChild(2).GetComponent<Renderer>().sortingOrder = index + 2;
    }
}
