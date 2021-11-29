using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
[CustomEditor(typeof(Field))]
public class FieldInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("field_type"));
        int propertyField = serializedObject.FindProperty("field_type").enumValueIndex;
        Field field = (Field) target;
        Sprite[] fieldIcon = Resources.LoadAll<Sprite>("FieldIcon");
        switch ((FieldType) propertyField)
        {
            case FieldType.Battle:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("monster_difficulty"));
                //field.spriteRenderer.sprite = fieldIcon[1];
                break;
            case FieldType.Event:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("event_type"));
                //field.spriteRenderer.sprite = fieldIcon[2];
                break;
            case FieldType.Rest:
                //field.spriteRenderer.sprite = fieldIcon[3];
                break;
            case FieldType.Shop:
                //field.spriteRenderer.sprite = fieldIcon[4];
                break;
            case FieldType.Boss:
                //field.spriteRenderer.sprite = fieldIcon[0];
                break;
        }

        string titleTxt = string.Empty;
        switch (propertyField)
        {
            case (int) FieldType.Battle:
                int monsterDifficulty = serializedObject.FindProperty("monster_difficulty").enumValueIndex;
                switch (monsterDifficulty)
                {
                    case (int) MONSTER_DIFFICULTY.EASY:
                        titleTxt = "이지";
                        break;
                    case (int) MONSTER_DIFFICULTY.NOMAL:
                        titleTxt = "노말";
                        break;
                    case (int) MONSTER_DIFFICULTY.HARD:
                        titleTxt = "하드";
                        break;
                }

                break;
            case (int) FieldType.Event:
                titleTxt = "이벤트";
                break;
            case (int) FieldType.Rest:
                titleTxt = "휴식";
                break;
            case (int) FieldType.Shop:
                titleTxt = "상점";
                break;
            case (int) FieldType.Map:
                break;
            case (int) FieldType.Boss:
                titleTxt = "보스";
                break;
        }

        field.transform.GetChild(0).GetComponent<TextMeshPro>().text = titleTxt;
        if (GUILayout.Button("이미지 변경"))
        {
            switch (propertyField)
            {
                case (int) FieldType.Battle:
                    field.SpriteRenderer.sprite = fieldIcon[1];
                    break;
                case (int) FieldType.Event:
                    field.SpriteRenderer.sprite = fieldIcon[2];
                    break;
                case (int) FieldType.Rest:
                    field.SpriteRenderer.sprite = fieldIcon[3];
                    break;
                case (int) FieldType.Shop:
                    field.SpriteRenderer.sprite = fieldIcon[4];
                    break;
                case (int) FieldType.Boss:
                    field.SpriteRenderer.sprite = fieldIcon[0];
                    break;
                default:
                    field.SpriteRenderer.sprite = fieldIcon[1];
                    break;
            }
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("isClear"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("clearObj"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("surroundingObj"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

[Serializable]
public class Field : MonoBehaviour
{
    [SerializeField] private FieldType field_type;
    [SerializeField] private MONSTER_DIFFICULTY monster_difficulty;

    private bool onField; //마우스가 필드 위에 있는지

    public List<GameObject> surroundingObj;

    public bool isReady;
    public bool isClear;
    public GameObject clearObj;

    public FieldData FieldData => new FieldData(field_type, GetMonster(monster_difficulty));

    public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        if (transform.childCount > 1)
        {
            clearObj = transform.GetChild(1).gameObject;
        }

        SpriteRenderer.color -= new Color(0, 0, 0, 0.5f);

        if (field_type == FieldType.Map)
        {
            SpriteRenderer.color += new Color(0, 0, 0, 0.5f);
            isReady = true;
        }
    }

    private void OnMouseUp()
    {
        if (onField && !isClear && isReady && !FadeManager.Inst.isActiveFade &&
            ThrowingObjManager.Inst.moveThrowingReward == 0)
        {
            if (MapManager.CurrentSceneName == "상점" && !ShopManager.Inst.isFinishTutorial)
            {
                return;
            }

            if (!MapManager.Inst.isFinishTutorialEventField && field_type == FieldType.Event)
            {
                return;
            }

            if (!MapManager.Inst.isTutorialBoss && monster_difficulty == MONSTER_DIFFICULTY.BOSS)
            {
                return;
            }

            MapManager.Inst.IconMouseUp(this);
        }
    }

    private void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
        onField = true;
    }

    private void OnMouseExit()
    {
        onField = false;
    }

    /// <summary>
    /// 필드 난이도에 따른 랜덤 몬스터 가져오가
    /// </summary>
    public MONSTER_TYPE GetMonster(MONSTER_DIFFICULTY difficulty)
    {
        var rand = Random.Range(0, 4);
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
                        return MONSTER_TYPE.WATER_WISP;
                    case 3:
                        return MONSTER_TYPE.WIND_WISP;
                }

                break;
            case MONSTER_DIFFICULTY.NOMAL:
                switch (rand)
                {
                    case 0:
                        return MONSTER_TYPE.EXECUTIONER;
                    case 1:
                        return MONSTER_TYPE.KOBOLD;
                    case 2:
                        return MONSTER_TYPE.REAPER;
                    case 3:
                        return MONSTER_TYPE.SHADE;
                }

                break;
            case MONSTER_DIFFICULTY.HARD:
                switch (rand)
                {
                    case 0:
                        return MONSTER_TYPE.FIRE_GOLEM;
                    case 1:
                        return MONSTER_TYPE.MINOTAUR;
                    case 2:
                        return MONSTER_TYPE.RED_OGRE;
                    case 3:
                        return MONSTER_TYPE.YETI;
                }

                break;
            case MONSTER_DIFFICULTY.BOSS:
                return MONSTER_TYPE.BOSS;
            default:
                throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
        }

        return MONSTER_TYPE.EARTH_WISP;
    }

    public void UpdateTypeText()
    {
        var fieldTxt = string.Empty;
        switch (field_type)
        {
            case FieldType.Battle:
                switch (monster_difficulty)
                {
                    case MONSTER_DIFFICULTY.EASY:
                        fieldTxt = "이지";
                        break;
                    case MONSTER_DIFFICULTY.NOMAL:
                        fieldTxt = "노말";
                        break;
                    case MONSTER_DIFFICULTY.HARD:
                        fieldTxt = "하드";
                        break;
                }

                break;
            case FieldType.Event:
                fieldTxt = "이벤트";
                break;
            case FieldType.Rest:
                fieldTxt = "휴식";
                break;
            case FieldType.Shop:
                fieldTxt = "상점";
                break;
            case FieldType.Map:
                break;
            case FieldType.Boss:
                fieldTxt = "보스";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        transform.GetChild(0).GetComponent<TextMeshPro>().text = fieldTxt;
    }

    /// <summary>
    /// 클리어한 필드에 클리어 표시
    /// </summary>
    public void UpdateClearImage()
    {
        if (!isClear || (clearObj == null))
        {
            return;
        }

        clearObj.gameObject.SetActive(true);
        SpriteRenderer.color += new Color(0, 0, 0, 0.5f);
        foreach (GameObject surroundObj in surroundingObj)
        {
            surroundObj.GetComponent<Field>().SpriteRenderer.color += new Color(0, 0, 0, 0.5f);
            surroundObj.GetComponent<Field>().isReady = true;
        }
    }
}
