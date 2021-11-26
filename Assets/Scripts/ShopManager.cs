using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Inst;

    public List<ShopItem> items;

    public bool isFinishTutorial;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.SHOP);
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Start();
        }

        ChangePriceColor();
        CheckItemMax();
        if (!MapManager.Inst.isTutorialInShop)
        {
            MapManager.Inst.isTutorialInShop = true;
            isFinishTutorial = false;
            StartCoroutine(TutorialShopCoroutine());
        }
        else
        {
            isFinishTutorial = true;
        }
    }

    public void Click(ShopItem shopItem)        //상점에서 해당 아이템 클릭했을때 보상 지급
    {
        if (!isFinishTutorial)
        {
            return;
        }

        SoundManager.Inst.Play(SHOPSOUND.BUY);
        switch (shopItem.item.type)
        {
            case SHOPITEM_TYPE.CARD:
                CardManager.Inst.AddCardDeck(shopItem.item.index);
                if (PlayerManager.Inst.question_card > 0)
                {
                    PlayerManager.Inst.question_card -= 1;
                }
                else
                {
                    PlayerManager.Inst.card_piece -= shopItem.item.price;
                }

                ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.NumCard, shopItem.transform.position, TopBar.Inst.GetIcon(TOPBAR_TYPE.BAG).transform.position, null, 1, 1, shopItem.item.index);
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

    private IEnumerator TutorialShopCoroutine()
    {
        yield return new WaitForSeconds(1);
        TalkWindow.Inst.InitFlag();
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[12].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(new Vector3(-2.37f, -0.7f, -5), ArrowCreateDirection.Down);
            }
            else if (i == 2)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(new Vector3(5.6f, 0, -5), ArrowCreateDirection.Right);
            }
            else if (i == 3)
            {
                ArrowManager.Inst.DestoryAllArrow();
            }
            else if (i == 4)
            {
                ArrowManager.Inst.CreateArrowObj(new Vector3(7.5f, -3, -5), ArrowCreateDirection.Up);
            }
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(12, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishTutorial = true;
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
