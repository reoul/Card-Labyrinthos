using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int original_Num { get; private set; }
    int _final_Num;
    public int final_Num
    {
        get
        {
            return _final_Num;
        }
        private set
        {
            _final_Num = Mathf.Clamp(value, 0, 5);
            UpdateNumTMP();
        }
    }
    [SerializeField] TMP_Text num_TMP;

    public PRS originPRS;
    public Transform parent;
    public bool isFinish = false;

    public void Setup(int num)
    {
        this.original_Num = num;
        this.final_Num = num;
        UpdateNumTMP();
    }

    public void UpdateNumTMP()
    {
        num_TMP.text = (final_Num + 1).ToString();
    }

    private void Awake()
    {
        parent = transform.parent;
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.parent.DOMove(prs.pos, dotweenTime);
            transform.parent.DORotateQuaternion(prs.rot, dotweenTime);
            transform.parent.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.parent.position = prs.pos;
            transform.parent.rotation = prs.rot;
            transform.parent.localScale = prs.scale;
        }
    }
    public void FinishCard()
    {
        parent.localScale = Vector3.one * 0.1f;
        SetActiveChildObj(false);
    }

    public void SetActiveChildObj(bool isActive)
    {
        this.parent.GetChild(1).gameObject.SetActive(isActive);
        this.parent.GetChild(2).gameObject.SetActive(isActive);
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
        if (!FadeManager.Inst.isActiveFade && !isFinish)
        {
            CardManager.Inst.CardMouseDown();
        }
    }

    void OnMouseUp()
    {
        if (!isFinish)
            CardManager.Inst.CardMouseUp();
    }

    public void Use(GameObject obj = null)
    {
        if (obj.tag == "Player")
            Player.Inst.hpbar.Sheld(final_Num + 1);
        else
        {
            Player.Inst.Attack();
            //EnemyManager.Inst.enemys[0].Damage(final_Num == obj.GetComponent<Enemy>().weaknessNum ? final_Num + 1 : 1);
        }
    }

    public void FinishScene()
    {
        isFinish = true;
        MoveTransform(originPRS, false);
        MoveTransform(new PRS(originPRS.pos - Vector3.up * 3, originPRS.rot, originPRS.scale), true, 0.3f);
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
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0 : 1);
        this.parent.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, isHalf ? 0.5f : 1);     //카드 앞면
        this.parent.GetChild(2).GetComponent<TMP_Text>().color = new Color(0, 0, 0, isHalf ? 0.5f : 1);     //숫자 텍스트
    }
}
