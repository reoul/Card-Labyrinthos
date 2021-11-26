using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int original_Num { get; private set; }
    int _final_Num;

    public int final_Num
    {
        get => this._final_Num;
        private set
        {
            this._final_Num = Mathf.Clamp(value, 0, 5);
            this.UpdateNumTMP();
        }
    }

    public TMP_Text num_TMP;

    public PRS originPRS;
    public Transform parent;
    public bool isFinish;
    bool _isLock;

    public bool isLock
    {
        get { return this._isLock; }
        private set { this._isLock = value; }
    }

    // TODO : 지금 이렇게
    public void Setup(int num)
    {
        this.original_Num = num;
        this.final_Num = num;
        this.UpdateNumTMP();
    }

    public void RevertOriginNum()
    {
        this.final_Num = this.original_Num;
    }

    public void UpdateNumTMP()
    {
        this.num_TMP.text = (this.final_Num + 1).ToString();
    }

    private void Awake()
    {
        this.parent = this.transform.parent;
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            this.transform.parent.DOMove(prs.pos, dotweenTime);
            this.transform.parent.DORotateQuaternion(prs.rot, dotweenTime);
            this.transform.parent.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            this.transform.parent.position = prs.pos;
            this.transform.parent.rotation = prs.rot;
            this.transform.parent.localScale = prs.scale;
        }
    }

    public void FinishCard()
    {
        this.parent.localScale = Vector3.one * 0.1f;
        this.SetActiveChildObj(false);
    }

    public void SetActiveChildObj(bool isActive)
    {
        this.parent.GetChild(1).gameObject.SetActive(isActive);
        this.parent.GetChild(2).gameObject.SetActive(isActive);
    }

    private void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
    }

    void OnMouseOver()
    {
        CardManager.Inst.CardMouseOver(this);
    }

    void OnMouseExit()
    {
        CardManager.Inst.CardMouseExit(this);
    }

    void OnMouseDown()
    {
        if (!FadeManager.Inst.isActiveFade && !this.isFinish && !this.isLock)
        {
            CardManager.Inst.CardMouseDown();
        }
    }

    void OnMouseUp()
    {
        if (!this.isFinish && !this.isLock)
            CardManager.Inst.CardMouseUp();
    }

    public void Use(GameObject obj = null)
    {
        if (obj.tag == "Player")
            Player.Inst.Sheld(this.final_Num + 1);
        else
        {
            Player.Inst.Attack();
            //EnemyManager.Inst.enemys[0].Damage(final_Num == obj.GetComponent<Enemy>().weaknessNum ? final_Num + 1 : 1);
        }
    }

    public void FinishScene()
    {
        this.isFinish = true;
        this.MoveTransform(this.originPRS, false);
        this.MoveTransform(new PRS(this.originPRS.pos - Vector3.up * 3, this.originPRS.rot, this.originPRS.scale), true,
            0.3f);
    }

    public void AddNum(int index)
    {
        this.final_Num += index;
    }

    public void SetFinalNum(int index)
    {
        this.final_Num = index;
    }

    public void SetColorAlpha(bool isHalf)
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0 : 1);
        this.parent.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0.5f : 1); //카드 앞면
        this.parent.GetChild(2).GetComponent<TMP_Text>().color = new Color(0, 0, 0, isHalf ? 0.5f : 1); //숫자 텍스트
    }

    public void Lock()
    {
        this.isLock = true;
    }

    public void UnLock()
    {
        this.isLock = false;
    }

    public void SetOrderLayer(int index)
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = index;
        this.transform.parent.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = index + 1;
        this.transform.parent.GetChild(2).GetComponent<Renderer>().sortingOrder = index + 2;
    }
}
