using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int original_Num { get; private set; }
    public int final_Num { get; private set; }
    [SerializeField] TMP_Text num_TMP;

    public PRS originPRS;
    public Transform parent;
    int a = 10;

    public void Setup(int num)
    {
        a = 20;
        this.original_Num = num;
        this.final_Num = num;
        num_TMP.text = (num + 1).ToString();
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
        //SetActiveChildObj(false);
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
        if (!FadeManager.Inst.isActiveFade)
            CardManager.Inst.CardMouseDown();
    }

    void OnMouseUp()
    {
        CardManager.Inst.CardMouseUp();
    }

    public void Use(GameObject obj = null)
    {
        if (obj.tag == "Player")
            Player.Inst.hpbar.Sheld(final_Num + 1);
        else
        {
            Player.Inst.Attack();
            EnemyManager.Inst.enemys[0].Damage(final_Num == obj.GetComponent<Enemy>().weaknessNum ? final_Num + 1 : 1);
        }
    }

    public void FinishScene()
    {
        MoveTransform(originPRS, false);
        MoveTransform(new PRS(originPRS.pos - Vector3.up * 3, originPRS.rot, originPRS.scale), true, 0.3f);
    }
}
