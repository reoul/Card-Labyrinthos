using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Field))]
public class FieldInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("field_type"));
        int propertyfield = serializedObject.FindProperty("field_type").enumValueIndex;
        Field field = (Field)target;
        Sprite[] fieldIcon = Resources.LoadAll<Sprite>("FieldIcon");
        switch (propertyfield)
        {
            case (int)FIELD_TYPE.BATTLE:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("monster_difficulty"));
                field.spriteRenderer.sprite = fieldIcon[1];
                break;
            case (int)FIELD_TYPE.EVENT:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("event_type"));
                field.spriteRenderer.sprite = fieldIcon[2];
                break;
            case (int)FIELD_TYPE.REST:
                field.spriteRenderer.sprite = fieldIcon[3];
                break;
            case (int)FIELD_TYPE.SHOP:
                field.spriteRenderer.sprite = fieldIcon[4];
                break;
            case (int)FIELD_TYPE.BOSS:
                field.spriteRenderer.sprite = fieldIcon[0];
                break;
            default:
                field.spriteRenderer.sprite = fieldIcon[1];
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}

[System.Serializable]
public class Field : MonoBehaviour
{
    [SerializeField]
    FIELD_TYPE field_type;
    [SerializeField]
    EVENT_TYPE event_type;
    [SerializeField]
    MONSTER_DIFFICULTY monster_difficulty;

    bool onField = false;   //마우스가 필드 위에 있는지

    public FieldData fielddata { get { return new FieldData(field_type, event_type, GetMonster(monster_difficulty)); } }

    public SpriteRenderer spriteRenderer => this.GetComponent<SpriteRenderer>();

    void OnMouseUp()
    {
        if (onField)
            MapManager.Inst.IconMouseUp(fielddata);
    }

    void OnMouseEnter()
    {
        onField = true;
    }

    void OnMouseExit()
    {
        onField = false;
    }

    public MONSTER_TYPE GetMonster(MONSTER_DIFFICULTY difficulty)       //필드 난이도에 따라 랜덤 몬스터 소환
    {
        int rand = Random.Range(0, difficulty == MONSTER_DIFFICULTY.EASY ? 10 : difficulty == MONSTER_DIFFICULTY.NOMAL ? 10 : 10);
        switch (difficulty)
        {
            case MONSTER_DIFFICULTY.EASY:
                switch (rand)
                {
                    case 0:
                        return MONSTER_TYPE.EARTH_WISP;
                    case 1:
                        return MONSTER_TYPE.FIRE_WISP;
                    case 2:
                        return MONSTER_TYPE.IMP;
                    case 3:
                        return MONSTER_TYPE.MANDRAKE;
                    case 4:
                        return MONSTER_TYPE.RAT;
                    case 5:
                        return MONSTER_TYPE.SLUG;
                    case 6:
                        return MONSTER_TYPE.UNDEAD_WARRIOR;
                    case 7:
                        return MONSTER_TYPE.WASP;
                    case 8:
                        return MONSTER_TYPE.WATER_WISP;
                    case 9:
                        return MONSTER_TYPE.WIND_WISP;
                }
                break;
            case MONSTER_DIFFICULTY.NOMAL:
                switch (rand)
                {
                    case 0:
                        return MONSTER_TYPE.DJINN_BANDIT;
                    case 1:
                        return MONSTER_TYPE.EXECUTIONER;
                    case 2:
                        return MONSTER_TYPE.GHOUL;
                    case 3:
                        return MONSTER_TYPE.GOBLIN;
                    case 4:
                        return MONSTER_TYPE.KOBOLD;
                    case 5:
                        return MONSTER_TYPE.MIMIC;
                    case 6:
                        return MONSTER_TYPE.OCULLOTHORAX;
                    case 7:
                        return MONSTER_TYPE.REAPER;
                    case 8:
                        return MONSTER_TYPE.SATYR;
                    case 9:
                        return MONSTER_TYPE.SHADE;
                }
                break;
            case MONSTER_DIFFICULTY.HARD:
                switch (rand)
                {
                    case 0:
                        return MONSTER_TYPE.FIRE_GOLEM;
                    case 1:
                        return MONSTER_TYPE.GOLEM;
                    case 2:
                        return MONSTER_TYPE.ICE_GOLEM;
                    case 3:
                        return MONSTER_TYPE.MINOTAUR;
                    case 4:
                        return MONSTER_TYPE.OGRE;
                    case 5:
                        return MONSTER_TYPE.RED_OGRE;
                    case 6:
                        return MONSTER_TYPE.WEREWOLF;
                    case 7:
                        return MONSTER_TYPE.YETI;
                    case 8:
                        return MONSTER_TYPE.NECROMANCER;
                    case 9:
                        return MONSTER_TYPE.PHANTOM_KNIGHT;
                }
                break;
        }
        return MONSTER_TYPE.IMP;
    }
}
