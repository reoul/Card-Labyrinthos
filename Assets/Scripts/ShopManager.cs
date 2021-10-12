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

    }
}
