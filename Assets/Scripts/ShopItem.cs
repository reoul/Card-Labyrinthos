using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHOPITEM_TYPE { CARD, SKILLPAGE, ADD_DRAW }

public class Item
{
    public SHOPITEM_TYPE type;
    public int index;
}

public class ShopItem : MonoBehaviour
{
    public Item item;
    bool onItem = false;    //마우스가 아이템 위에 있는지

    private void OnMouseUp()
    {
        if (onItem)
            Debug.Log("aadsads");
        //ShopManager.Inst.Click(this.item);
    }

    private void OnMouseEnter()
    {
        onItem = true;
    }

    private void OnMouseExit()
    {
        onItem = false;
    }
}
