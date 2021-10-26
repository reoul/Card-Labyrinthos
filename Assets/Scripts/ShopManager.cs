using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Inst;

    public List<ShopItem> items;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Start();
        }
        ChangePriceColor();
        CheckItemMax();
    }

    public void Click(Item item)        //상점에서 해당 아이템 클릭했을때 보상 지급
    {
        switch (item.type)
        {
            case SHOPITEM_TYPE.CARD:
                CardManager.Inst.AddCardDeck(item.index);
                break;
            case SHOPITEM_TYPE.ADD_DRAW:
                TurnManager.Inst.AddStartTurnCard();
                break;
        }
        PlayerManager.Inst.card_piece -= item.price;
        ChangePriceColor();
        CheckItemMax();
    }

    public void ChangePriceColor()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].ChangePriceColor();
        }
    }

    public void CheckItemMax()
    {
        bool[] cardmax = CardManager.Inst.isCardDeckMax();
        for (int i = 0; i < cardmax.Length; i++)
        {
            if (cardmax[i])
            {
                items[i].transform.GetChild(1).gameObject.SetActive(true);
                items[i].CountMax();
            }
        }
        if (TurnManager.Inst.isStartCardCountMax)
        {
            items[6].transform.GetChild(1).gameObject.SetActive(true);
            items[6].CountMax();
        }
    }
}
