using System;
using UnityEngine;

public enum MONSTER_TYPE
{
    Goblin,
    Kobold,
    Ogre,
    Ocullothorax,
    Mimic,
    EarthWisp,
    WindWisp,
    FireWisp,
    WaterWisp,
    Minotaur,
    Rat,
    Mandrake,
    DjinnBandit,
    Satyr,
    Shade,
    Wasp,
    Werewolf,
    Yeti,
    Golem,
    Executioner,
    FireGolem,
    Ghoul,
    IceGolem,
    Imp,
    Necromancer,
    PhantomKnight,
    Reaper,
    Slug,
    UndeadWarrior,
    NomalChest,
    RedOgre,
    Boss
}

public enum MONSTER_DIFFICULTY
{
    Easy,
    Nomal,
    Hard,
    Boss
}

public enum PATTERN_TYPE
{
    Attack,
    Heal
}

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

[Serializable]
public class Monster
{
    public string name;
    public int hp;
    public MONSTER_TYPE type;
    public GameObject prefab;

    public PATTERN pattern_1;
    public PATTERN pattern_2;
    public PATTERN pattern_3;
    public PATTERN pattern_4;

    public float attackDelay;
}

[Serializable]
[CreateAssetMenu(fileName = "MonsterSO", menuName = "Scriptable Object/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    public Monster[] monsters;
}
