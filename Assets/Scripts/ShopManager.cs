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

    public void Init()
    {

    }

    public void Click(Item item)
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
    }

    public void CheckItemLimit()
    {
        //TurnManager.Inst.
    }
}
