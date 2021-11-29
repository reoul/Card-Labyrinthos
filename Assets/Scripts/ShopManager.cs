using System;
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
        SoundManager.Inst.Play(BACKGROUNDSOUND.Shop);
        foreach (ShopItem item in items)
        {
            item.Start();
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

    /// <summary>
    /// 상점에서 아이템을 클릭했을때 보상 지급
    /// </summary>
    public void Click(ShopItem shopItem)
    {
        if (!isFinishTutorial)
        {
            return;
        }

        SoundManager.Inst.Play(SHOPSOUND.Buy);
        switch (shopItem.item.type)
        {
            case SHOPITEM_TYPE.Card:
                CardManager.Inst.AddCardDeck(shopItem.item.index);
                if (PlayerManager.Inst.QuestionCard > 0)
                {
                    PlayerManager.Inst.QuestionCard -= 1;
                }
                else
                {
                    PlayerManager.Inst.CardPiece -= shopItem.item.price;
                }

                ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.NumCard, shopItem.transform.position,
                    TopBar.Inst.GetIcon(TOPBAR_TYPE.BAG).transform.position, null, 1, 1, shopItem.item.index);
                break;
            case SHOPITEM_TYPE.AddDraw:
                if (PlayerManager.Inst.CardPiece >= shopItem.item.price)
                {
                    TurnManager.Inst.AddStartTurnCard();
                    PlayerManager.Inst.CardPiece -= shopItem.item.price;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        ChangePriceColor();
        CheckItemMax();
    }

    private void ChangePriceColor()
    {
        foreach (ShopItem item in items)
        {
            item.ChangePriceColor();
        }
    }

    private IEnumerator TutorialShopCoroutine()
    {
        yield return new WaitForSeconds(1);
        TalkWindow.Inst.InitFlag();
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (var i = 0; i < TalkWindow.Inst.talks[12].Count; i++)
        {
            switch (i)
            {
                case 0:
                    ArrowManager.Inst.CreateArrowObj(new Vector3(-2.37f, -0.7f, -5), ArrowCreateDirection.Down);
                    break;
                case 2:
                    ArrowManager.Inst.DestroyAllArrow();
                    ArrowManager.Inst.CreateArrowObj(new Vector3(5.6f, 0, -5), ArrowCreateDirection.Right);
                    break;
                case 3:
                    ArrowManager.Inst.DestroyAllArrow();
                    break;
                case 4:
                    ArrowManager.Inst.CreateArrowObj(new Vector3(7.5f, -3, -5), ArrowCreateDirection.Up);
                    break;
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(12, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }

        ArrowManager.Inst.DestroyAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishTutorial = true;
    }

    private void CheckItemMax()
    {
        bool[] cardmax = CardManager.Inst.IsCardDeckMax();
        for (var i = 0; i < cardmax.Length; i++)
        {
            if (cardmax[i])
            {
                items[i].transform.GetChild(1).gameObject.SetActive(true);
                items[i].CountMax();
            }
        }

        if (TurnManager.Inst.IsStartCardCountMax)
        {
            items[6].transform.GetChild(1).gameObject.SetActive(true);
            items[6].CountMax();
        }
    }
}
