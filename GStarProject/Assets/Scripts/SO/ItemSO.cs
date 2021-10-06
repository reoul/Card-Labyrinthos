using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Designated,
    NonDesignated
}

[System.Serializable]
public class Item
{
    public string name; //카드 이름
    [Range(0,3)]
    public int cost;    //해당 카드 비용
    public CardType type;
    public Sprite sprite;
    [Header("공격")]
    public int attack;  //해당수만큼 적에게 데미지를 입힘
    public int attack_count;    //공격 횟수
    public bool allememy;   //전체 적에게 데미지를 줌

    [Header("방어")]
    public int shield;  //해당수만큼 방어막을얻음 ex)방어막이 3인데 공격력이 4이면 내 캐릭터에게 1만큼의 데미지 들감

    [Header("유틸")]
    public bool copy;   //원하는 카드를 한장 복사해 손으로 가져온다
    public bool copy_throw;   //원하는 카드를 한장 복사해 버린 카드 더미에 둔다
    public int get_cost;    //해당 수 만큼 코스트를 얻는다
    public int less_damage; //적의 공격력에 해당 수만큼 줄임
    public int draw_card;   //해당 수만큼 카드를 드로우 해온다
    public int force;   //해당 수만큼 공격력이 올라간다.
    public int vulnerable;  //해당 수만큼 취약을 건다. 취약: 데미지를 받을때 50퍼 더 많이 받는다
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject
{
    public Item[] items;
}
