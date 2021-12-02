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
        var field = (Field) target;
        var fieldIcon = Resources.LoadAll<Sprite>("FieldIcon");
        switch ((FieldType) propertyField)
        {
            case FieldType.Battle:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("monster_difficulty"));
                break;
            case FieldType.Event:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("event_type"));
                break;
            case FieldType.Rest:
                break;
            case FieldType.Shop:
                break;
            case FieldType.Boss:
                break;
        }

        var titleTxt = string.Empty;
        switch (propertyField)
        {
            case (int) FieldType.Battle:
                int monsterDifficulty = serializedObject.FindProperty("monster_difficulty").enumValueIndex;
                switch (monsterDifficulty)
                {
                    case (int) MONSTER_DIFFICULTY.Easy:
                        titleTxt = "이지";
                        break;
                    case (int) MONSTER_DIFFICULTY.Nomal:
                        titleTxt = "노말";
                        break;
                    case (int) MONSTER_DIFFICULTY.Hard:
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
public class Field : MouseInteractionObject
{
    [SerializeField] private FieldType field_type;
    [SerializeField] private MONSTER_DIFFICULTY monster_difficulty;

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
        if (OnMouse && !isClear && isReady && !FadeManager.Inst.isActiveFade &&
            ThrowingObjManager.Inst.MoveThrowingReward == 0)
        {
            if (MapManager.CurrentSceneName == "상점" && !ShopManager.Inst.isFinishTutorial)
            {
                return;
            }

            if (!MapManager.Inst.isFinishTutorialEventField && field_type == FieldType.Event)
            {
                return;
            }

            if (!MapManager.Inst.isTutorialBoss && monster_difficulty == MONSTER_DIFFICULTY.Boss)
            {
                return;
            }

            MapManager.Inst.IconMouseUp(this);
        }
    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
    }

    /// <summary>
    /// 필드 난이도에 따른 랜덤 몬스터 가져오가
    /// </summary>
    public MONSTER_TYPE GetMonster(MONSTER_DIFFICULTY difficulty)
    {
        var rand = Random.Range(0, 4);
        switch (difficulty)
        {
            case MONSTER_DIFFICULTY.Easy:
                switch (rand)
                {
                    case 0:
                        return MONSTER_TYPE.EarthWisp;
                    case 1:
                        return MONSTER_TYPE.FireWisp;
                    case 2:
                        return MONSTER_TYPE.WaterWisp;
                    case 3:
                        return MONSTER_TYPE.WindWisp;
                }

                break;
            case MONSTER_DIFFICULTY.Nomal:
                switch (rand)
                {
                    case 0:
                        return MONSTER_TYPE.Executioner;
                    case 1:
                        return MONSTER_TYPE.Kobold;
                    case 2:
                        return MONSTER_TYPE.Reaper;
                    case 3:
                        return MONSTER_TYPE.Shade;
                }

                break;
            case MONSTER_DIFFICULTY.Hard:
                switch (rand)
                {
                    case 0:
                        return MONSTER_TYPE.FireGolem;
                    case 1:
                        return MONSTER_TYPE.Minotaur;
                    case 2:
                        return MONSTER_TYPE.RedOgre;
                    case 3:
                        return MONSTER_TYPE.Yeti;
                }

                break;
            case MONSTER_DIFFICULTY.Boss:
                return MONSTER_TYPE.Boss;
            default:
                throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
        }

        return MONSTER_TYPE.EarthWisp;
    }

    public void UpdateTypeText()
    {
        var fieldTxt = string.Empty;
        switch (field_type)
        {
            case FieldType.Battle:
                switch (monster_difficulty)
                {
                    case MONSTER_DIFFICULTY.Easy:
                        fieldTxt = "이지";
                        break;
                    case MONSTER_DIFFICULTY.Nomal:
                        fieldTxt = "노말";
                        break;
                    case MONSTER_DIFFICULTY.Hard:
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
            case FieldType.Tutorial:
                break;
            case FieldType.Tutorial2:
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
