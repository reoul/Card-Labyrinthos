using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public enum MONSTER_TYPE { GOBLIN, KOBOLD, OGRE, OCULLOTHORAX, MIMIC, EARTH_WISP, WIND_WISP, FIRE_WISP, WATER_WISP, MINOTAUR, RAT, MANDRAKE, DJINN_BANDIT, SATYR, SHADE, WASP, WEREWOLF, YETI, GOLEM, EXECUTIONER, FIRE_GOLEM, GHOUL, ICE_GOLEM, IMP, NECROMANCER, PHANTOM_KNIGHT, REAPER, SLUG, UNDEAD_WARRIOR }

public enum PATTERN_TYPE { ATTACK, HEAL, EVENT1, EVENT2 }

[System.Serializable]
public class PATTERN
{
    public PATTERN_TYPE pattern_type;
    public int index;
}

public enum CardType
{
    Designated,
    NonDesignated
}

[System.Serializable]
public class Monster
{
    public string name; //���� �̸�
    public int hp;    //���� ü��
    public MONSTER_TYPE type;   //Ÿ��
    public GameObject prefab;   //���� ������

    [Header("����")]
    public PATTERN pattern_1;
    public PATTERN pattern_2;
    public PATTERN pattern_3;
    public PATTERN pattern_4;
    public PATTERN pattern_5;
    public PATTERN pattern_6;
}

[CreateAssetMenu(fileName = "MonsterSO", menuName = "Scriptable Object/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    public Monster[] monsters;
}