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
    private bool onItem;    //마우스가 아이템 위에 있는지
    private Vector3 originalScale;
    private bool isMax;
    [SerializeField] private GameObject bottomBar;
    [SerializeField] private TMP_Text priceTMP;

    public void Start()
    {
        originalScale = transform.localScale == Vector3.zero ? Vector3.one * 0.35f : transform.localScale;
    }

    private void OnMouseUp()
    {
        if (onItem && !FadeManager.Inst.isActiveFade && !isMax)
        {
            if (PlayerManager.Inst.card_piece >= item.price || PlayerManager.Inst.question_card > 0)
            {
                ShopManager.Inst.Click(this);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!isMax && !FadeManager.Inst.isActiveFade)
        {
            SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
            onItem = true;
            transform.localScale = originalScale + Vector3.one * 0.02f;
        }
    }

    private void OnMouseExit()
    {
        onItem = false;
        transform.localScale = originalScale;
    }

    public void ChangePriceColor()
    {
        if (PlayerManager.Inst.card_piece >= item.price)
        {
            priceTMP.color = Color.green;
        }
        else
        {
            priceTMP.color = Color.red;
        }
    }

    public void CountMax()
    {
        isMax = true;
        transform.localScale = originalScale;
        bottomBar.SetActive(false);
    }
}
