using System;
using UnityEngine;

public enum MONSTER_TYPE
{
    GOBLIN, KOBOLD, OGRE, OCULLOTHORAX, MIMIC, EARTH_WISP, WIND_WISP, FIRE_WISP, WATER_WISP, MINOTAUR,
    RAT, MANDRAKE, DJINN_BANDIT, SATYR, SHADE, WASP, WEREWOLF, YETI, GOLEM, EXECUTIONER, FIRE_GOLEM, GHOUL,
    ICE_GOLEM, IMP, NECROMANCER, PHANTOM_KNIGHT, REAPER, SLUG, UNDEAD_WARRIOR, NOMAL_CHEST, RED_OGRE, BOSS
}

public enum MONSTER_DIFFICULTY { EASY, NOMAL, HARD, BOSS }

public enum PATTERN_TYPE { ATTACK, HEAL }

[Serializable]
public class PATTERN
{
    public PATTERN_TYPE pattern_type;
    public int index;

    public PATTERN(PATTERN_TYPE type, int index = 0)
    {
        pattern_type = type;
        this.index = index;
    }
}

public enum CardType
{
    Designated,
    NonDesignated
}

[Serializable]
public class Monster
{
    public string name; //¸ó½ºÅÍ ÀÌ¸§
    public int hp;    //¸ó½ºÅÍ Ã¼·Â
    public MONSTER_TYPE type;   //Å¸ÀÔ
    public GameObject prefab;   //¸ó½ºÅÍ ÇÁ¸®ÆÕ

    [Header("ÆÐÅÏ")]
    public PATTERN pattern_1; //¾à°ø
    public PATTERN pattern_2; //Áß°ø
    public PATTERN pattern_3; //°­°ø
    public PATTERN pattern_4; //È¸º¹

    public float attackDelay;
}

[Serializable]
[CreateAssetMenu(fileName = "MonsterSO", menuName = "Scriptable Object/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    public Monster[] monsters;
}
