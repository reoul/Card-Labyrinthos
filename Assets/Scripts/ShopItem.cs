using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHOPITEM_TYPE { CARD, ADD_DRAW }

[System.Serializable]
public class Item
{
    public SHOPITEM_TYPE type;
    public int index;
}

public class ShopItem : MonoBehaviour
{
    public Item item;
    bool onItem = false;    //마우스가 아이템 위에 있는지
    Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnMouseUp()
    {
        ShopManager.Inst.Click(item);
    }

    private void OnMouseEnter()
    {
        onItem = true;
        transform.localScale = originalScale + Vector3.one * 0.1f;
    }

    private void OnMouseExit()
    {
        onItem = false;
        transform.localScale = originalScale;
    }
}
