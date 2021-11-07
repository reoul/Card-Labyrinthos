using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class FieldData
{
    public FIELD_TYPE field_type;
    public EVENT_TYPE event_type;
    public MONSTER_TYPE monster_type;

    public FieldData()
    {

    }

    public FieldData(FIELD_TYPE field_type, EVENT_TYPE event_type, MONSTER_TYPE monster_type)
    {
        this.field_type = field_type;
        this.event_type = event_type;
        this.monster_type = monster_type;
    }
}

public enum FIELD_TYPE { BATTLE, EVENT, REST, SHOP, MAP, BOSS, TUTORIAL, TUTORIAL2 }
public enum EVENT_TYPE { EVENT1, EVENT2, EVENT3 };

public class SceneEventArgs : EventArgs
{
    public FieldData fieldData { get; set; }
    public SceneEventArgs(FieldData fieldData)
    {
        this.fieldData = fieldData;
    }
}

public class MapManager : MonoBehaviour
{
    public static MapManager Inst = null;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public FieldData fieldData;
    [SerializeField]
    int selectFieldIndex;

    [SerializeField]
    bool[] isClear;     //필드 클리어 여부, 해당 필드가 끝이 날때 변경
    [SerializeField]
    GameObject fieldParent;
    public Field[] fields;

    bool isFinishToturial = false;

    bool isMoveCamera = false;
    Vector3 lastMousePos;

    public int tutorialIndex = 0;

    public string CurrentSceneName      //현재 씬 이름
    {
        get
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Map":
                    return "지도";
                case "Battle":
                case "Tutorial2":
                    return "전투";
                case "Event":
                    return "이벤트";
                case "Shop":
                    return "상점";
                case "Boss":
                    return "보스";
                case "Rest":
                    return "휴식";
                case "Tutorial":
                    return "알 수 없는 공간";
                case "Intro":
                    return "인트로";
            }
            return "지도";
        }
    }

    private void Start()
    {
        //fieldParent = GameObject.Find("FieldParent");
        //StartCoroutine(FieldClearCheckCorutine());
    }

    public void Init()
    {
        fieldData.field_type = FIELD_TYPE.BATTLE;
        fieldData.event_type = EVENT_TYPE.EVENT1;
        fieldData.monster_type = MONSTER_TYPE.RAT;
        selectFieldIndex = 0;
        isClear = new bool[0];
        fieldParent = null;
        fields = new Field[0];
    }

    public void IconMouseUp(Field field)
    {
        SoundManager.Inst.Play(MAPSOUND.CHOICE_FIELD);
        this.fieldData = field.fieldData;
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].Equals(field))
                selectFieldIndex = i;
        }
        FadeManager.FadeEvent += new EventHandler(LoadScene);
        StartEvent();
    }

    public void LoadScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene(fieldData.field_type.ToString());
    }

    public void LoadMapScene(bool clear)
    {
        if (isClear.Length != 0)
            isClear[selectFieldIndex] = clear;
        TurnManager.Inst.isFinish = false;
        fieldData.field_type = FIELD_TYPE.MAP;
        FadeManager.FadeEvent += new EventHandler(LoadScene);
        StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null, FieldClearCheckCorutine(), InitSkillTime(), null, isFinishToturial ? null : MapTutorialCorutine()));
    }

    public void LoadTutorialScene()
    {
        fieldData.field_type = FIELD_TYPE.TUTORIAL;
        FadeManager.FadeEvent += new EventHandler(LoadScene);
        StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null, null, null, null));
    }

    void StartEvent()
    {
        switch (fieldData.field_type)
        {
            case FIELD_TYPE.BATTLE:
                StartCoroutine(FadeManager.Inst.FadeInOut(TurnManager.Inst.ShowDebuffCorutine(), null, null,
                    PlayerManager.Inst.SetupGameCorutine(), null, null,
                        CardManager.Inst.InitCorutine(), TurnManager.Inst.StartGameCorutine()));
                break;
            case FIELD_TYPE.EVENT:
                StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    EventManager.Inst.RandomEventCorutine(), null, null,
                        CardManager.Inst.InitCorutine(), TurnManager.Inst.StartGameCorutine()));
                break;
            case FIELD_TYPE.SHOP:
                StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.REST:
                StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.MAP:
                StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    FieldClearCheckCorutine(), InitSkillTime()));
                break;
            case FIELD_TYPE.BOSS:
                StartCoroutine(FadeManager.Inst.FadeInOut(TurnManager.Inst.ShowDebuffCorutine(), null, null,
                    PlayerManager.Inst.SetupGameCorutine(), null, null,
                        CardManager.Inst.InitCorutine(), TurnManager.Inst.StartGameCorutine()));
                break;
            case FIELD_TYPE.TUTORIAL:
                StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    null, null, null,
                        TutorialManager.Inst.TutorialCorutine()));
                break;
            case FIELD_TYPE.TUTORIAL2:
                StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    PlayerManager.Inst.SetupGameCorutine(), null, null,
                        CardManager.Inst.FixedCardNumToturial2Corutine(), CardManager.Inst.InitCorutine(), TurnManager.Inst.StartGameCorutine()));
                break;
        }
    }

    public IEnumerator FieldClearCheckCorutine()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.MAP);
        if (fieldParent == null)
            fieldParent = GameObject.Find("FieldParent");
        fields = fieldParent.GetComponentsInChildren<Field>(true);
        if (isClear.Length == 0)
            isClear = new bool[fields.Length];
        isClear[0] = true;

        for (int i = 0; i < fields.Length; i++)
        {
            fields[i].isClear = isClear[i];
            for (int j = 0; j < fields.Length; j++)
            {
                if (Vector3.Distance(fields[i].transform.position, fields[j].transform.position) < 2.5f &&
                        Vector3.Distance(fields[i].transform.position, fields[j].transform.position) >= 1.8f)
                    fields[i].surroundingObj.Add(fields[j].gameObject);
            }
            fields[i].UpdateClearImage();
        }
        yield return null;
    }

    public IEnumerator InitSkillTime()
    {
        SkillManager.Inst.InitSkillTime();
        yield return null;
    }

    public IEnumerator MapTutorialCorutine()
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[1].Count; i++)
        {
            if (i == 2)
                ArrowManager.Inst.CreateArrowObj(fields[0].transform.position + -Vector3.right, ArrowCreateDirection.LEFT, fields[0].transform);
            else if (i == 3)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(fields[1].transform.position + -Vector3.right, ArrowCreateDirection.LEFT, fields[1].transform);
            }
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCorutine(1, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCorutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCorutine());
        }
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishToturial = true;
        yield return null;
    }
}
