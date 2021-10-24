﻿using System.Collections;
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
        CheckItemMax();
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
        CheckItemMax();
    }

    public void CheckItemMax()
    {
        bool[] cardmax = CardManager.Inst.isCardDeckMax();
        for (int i = 0; i < cardmax.Length; i++)
        {
            if (cardmax[i])
            {
                items[i].transform.GetChild(2).gameObject.SetActive(true);
                items[i].CountMax();
            }
        }
        if (TurnManager.Inst.isStartCardCountMax)
        {
            items[6].transform.GetChild(2).gameObject.SetActive(true);
            items[6].CountMax();
        }
    }
}
