using System;
using TMPro;
using UnityEngine;

public enum SHOPITEM_TYPE { CARD, ADD_DRAW }

[Serializable]
public class Item
{
    public SHOPITEM_TYPE type;
    public int index;
    public int price;
}

public class ShopItem : MonoBehaviour
{
    public Item item;
    bool onItem;    //마우스가 아이템 위에 있는지
    Vector3 originalScale;
    bool isMax;
    [SerializeField] GameObject bottomBar;
    [SerializeField] TMP_Text priceTMP;

    public void Start()
    {
        this.originalScale = this.transform.localScale == Vector3.zero ? Vector3.one * 0.35f : this.transform.localScale;
    }

    private void OnMouseUp()
    {
        if (this.onItem && !FadeManager.Inst.isActiveFade && !this.isMax)
            if (PlayerManager.Inst.card_piece >= this.item.price || PlayerManager.Inst.question_card > 0)
                ShopManager.Inst.Click(this);
    }

    private void OnMouseEnter()
    {
        if (!this.isMax && !FadeManager.Inst.isActiveFade)
        {
            SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
            this.onItem = true;
            this.transform.localScale = this.originalScale + Vector3.one * 0.02f;
        }
    }

    private void OnMouseExit()
    {
        this.onItem = false;
        this.transform.localScale = this.originalScale;
    }

    public void ChangePriceColor()
    {
        if (PlayerManager.Inst.card_piece >= this.item.price)
            this.priceTMP.color = Color.green;
        else
            this.priceTMP.color = Color.red;
    }

    public void CountMax()
    {
        this.isMax = true;
        this.transform.localScale = this.originalScale;
        this.bottomBar.SetActive(false);
    }
}
