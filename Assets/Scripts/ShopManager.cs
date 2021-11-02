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
        HelpManager.Inst.ShowHelp(HELP_TYPE.SHOP);
        SoundManager.Inst.Play(BACKGROUNDSOUND.SHOP);
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Start();
        }
        ChangePriceColor();
        CheckItemMax();
    }

    public void Click(ShopItem shopItem)        //상점에서 해당 아이템 클릭했을때 보상 지급
    {
        SoundManager.Inst.Play(SHOPSOUND.BUY);
        switch (shopItem.item.type)
        {
            case SHOPITEM_TYPE.CARD:
                CardManager.Inst.AddCardDeck(shopItem.item.index);
                if (PlayerManager.Inst.question_card > 0)
                    PlayerManager.Inst.question_card -= 1;
                else
                    PlayerManager.Inst.card_piece -= shopItem.item.price;
                ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.NUM_CARD, shopItem.transform.position, TopBar.Inst.GetIcon(TOPBAR_TYPE.BAG).transform.position, null, 1, 1, shopItem.item.index);
                break;
            case SHOPITEM_TYPE.ADD_DRAW:
                if (PlayerManager.Inst.card_piece >= shopItem.item.price)
                {
                    TurnManager.Inst.AddStartTurnCard();
                    PlayerManager.Inst.card_piece -= shopItem.item.price;
                }
                break;
        }
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
