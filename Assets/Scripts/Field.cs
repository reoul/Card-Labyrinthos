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
        switch ((FIELD_TYPE) propertyField)
        {
            case FIELD_TYPE.BATTLE:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("monster_difficulty"));
                //field.spriteRenderer.sprite = fieldIcon[1];
                break;
            case FIELD_TYPE.EVENT:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("event_type"));
                //field.spriteRenderer.sprite = fieldIcon[2];
                break;
            case FIELD_TYPE.REST:
                //field.spriteRenderer.sprite = fieldIcon[3];
                break;
            case FIELD_TYPE.SHOP:
                //field.spriteRenderer.sprite = fieldIcon[4];
                break;
            case FIELD_TYPE.BOSS:
                //field.spriteRenderer.sprite = fieldIcon[0];
                break;
        }

        string titleTxt = string.Empty;
        switch (propertyField)
        {
            case (int) FIELD_TYPE.BATTLE:
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
            case (int) FIELD_TYPE.EVENT:
                titleTxt = "이벤트";
                break;
            case (int) FIELD_TYPE.REST:
                titleTxt = "휴식";
                break;
            case (int) FIELD_TYPE.SHOP:
                titleTxt = "상점";
                break;
            case (int) FIELD_TYPE.MAP:
                break;
            case (int) FIELD_TYPE.BOSS:
                titleTxt = "보스";
                break;
        }

        field.transform.GetChild(0).GetComponent<TextMeshPro>().text = titleTxt;
        if (GUILayout.Button("이미지 변경"))
        {
            switch (propertyField)
            {
                case (int) FIELD_TYPE.BATTLE:
                    field.spriteRenderer.sprite = fieldIcon[1];
                    break;
                case (int) FIELD_TYPE.EVENT:
                    field.spriteRenderer.sprite = fieldIcon[2];
                    break;
                case (int) FIELD_TYPE.REST:
                    field.spriteRenderer.sprite = fieldIcon[3];
                    break;
                case (int) FIELD_TYPE.SHOP:
                    field.spriteRenderer.sprite = fieldIcon[4];
                    break;
                case (int) FIELD_TYPE.BOSS:
                    field.spriteRenderer.sprite = fieldIcon[0];
                    break;
                default:
                    field.spriteRenderer.sprite = fieldIcon[1];
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
    [SerializeField] private FIELD_TYPE field_type;
    [SerializeField] private EVENT_TYPE event_type;
    [SerializeField] private MONSTER_DIFFICULTY monster_difficulty;

    private bool onField; //마우스가 필드 위에 있는지

    public List<GameObject> surroundingObj;

    public bool isReady;
    public bool isClear;
    public bool isDebuffOff;
    public GameObject clearObj;

    public FieldData fieldData => new FieldData(field_type, event_type, GetMonster(monster_difficulty));

    public SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        if (transform.childCount > 1)
        {
            clearObj = transform.GetChild(1).gameObject;
        }

        spriteRenderer.color -= new Color(0, 0, 0, 0.5f);
        if (field_type == FIELD_TYPE.MAP)
        {
            spriteRenderer.color += new Color(0, 0, 0, 0.5f);
            isReady = true;
        }
    }

    private void OnMouseUp()
    {
        if (onField && !isClear && isReady && !FadeManager.Inst.isActiveFade &&
            ThrowingObjManager.Inst.moveThrowingReward == 0)
        {
            if (MapManager.Inst.CurrentSceneName == "상점" && !ShopManager.Inst.isFinishTutorial)
            {
                return;
            }

            if (!MapManager.Inst.isFinishTutorialEventField && field_type == FIELD_TYPE.EVENT)
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
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_MOUSEUP);
        onField = true;
    }

    private void OnMouseExit()
    {
        onField = false;
    }

    public MONSTER_TYPE GetMonster(MONSTER_DIFFICULTY difficulty) //필드 난이도에 따라 랜덤 몬스터 소환
    {
        //int rand = Random.Range(0, difficulty == MONSTER_DIFFICULTY.EASY ? 10 : difficulty == MONSTER_DIFFICULTY.NOMAL ? 9 : 10);
        int rand = Random.Range(0, 4);
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
        }

        return MONSTER_TYPE.EARTH_WISP;
    }

    public void UpdateTypeText()
    {
        string fieldTxt = string.Empty;
        switch (field_type)
        {
            case FIELD_TYPE.BATTLE:
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
            case FIELD_TYPE.EVENT:
                fieldTxt = "이벤트";
                break;
            case FIELD_TYPE.REST:
                fieldTxt = "휴식";
                break;
            case FIELD_TYPE.SHOP:
                fieldTxt = "상점";
                break;
            case FIELD_TYPE.MAP:
                break;
            case FIELD_TYPE.BOSS:
                fieldTxt = "보스";
                break;
        }

        transform.GetChild(0).GetComponent<TextMeshPro>().text = fieldTxt;
    }

    public void UpdateClearImage()
    {
        if (isClear && (clearObj != null))
        {
            clearObj.gameObject.SetActive(true);
            spriteRenderer.color += new Color(0, 0, 0, 0.5f);
            foreach (GameObject surroundObj in surroundingObj)
            {
                surroundObj.GetComponent<Field>().spriteRenderer.color += new Color(0, 0, 0, 0.5f);
                surroundObj.GetComponent<Field>().isReady = true;
            }
        }
    }
}
