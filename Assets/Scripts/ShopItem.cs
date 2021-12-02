using System;
using TMPro;
using UnityEngine;

public enum SHOPITEM_TYPE
{
    Card,
    AddDraw
}

[Serializable]
public class Item
{
    public SHOPITEM_TYPE type;
    public int index;
    public int price;
}

public class ShopItem : MouseInteractionObject
{
    public Item item;
    private Vector3 originalScale;
    private bool isMax;
    [SerializeField] private GameObject bottomBar;
    [SerializeField] private TMP_Text priceTMP;

    public void Start()
    {
        originalScale = (transform.localScale == Vector3.zero) ? Vector3.one * 0.35f : transform.localScale;
    }

    private void OnMouseUp()
    {
        if (!OnMouse || FadeManager.Inst.isActiveFade || isMax)
        {
            return;
        }

        if (PlayerManager.Inst.CardPiece >= item.price || PlayerManager.Inst.QuestionCard > 0)
        {
            ShopManager.Inst.Click(this);
        }
    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
        if (isMax || FadeManager.Inst.isActiveFade)
        {
            return;
        }

        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
        transform.localScale = originalScale + Vector3.one * 0.02f;
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();
        transform.localScale = originalScale;
    }

    /// <summary>
    /// 아이템 가격보다 돈이 많거나 같은 때 가격의 색깔을 초록색으로 설정한다. 아니면 빨간색으로 설정한다.
    /// </summary>
    public void ChangePriceColor()
    {
        priceTMP.color = (PlayerManager.Inst.CardPiece >= item.price) ? Color.green : Color.red;
    }

    public void CountMax()
    {
        isMax = true;
        transform.localScale = originalScale;
        bottomBar.SetActive(false);
    }
}
