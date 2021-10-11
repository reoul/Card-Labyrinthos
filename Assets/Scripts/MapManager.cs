using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public enum FIELD_TYPE { BATTLE, EVENT, REST, SHOP, MAP }
public enum EVENT_TYPE { EVENT1, EVENT2, EVENT3 };
public enum MONSTER_TYPE { MONSTER1, MONSTER2, MONSTER3 };

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
    public static MapManager Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public FieldData data;

    public void IconMouseUp(FieldData fieldData)
    {
        data = fieldData;
        FadeManager.FadeEvent += new EventHandler(LoadScene);
        StartEvent();
    }

    public void LoadScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene(data.field_type.ToString());
    }

    void StartEvent()
    {
        switch (data.field_type)
        {
            case FIELD_TYPE.BATTLE:
                StartCoroutine(FadeManager.Inst.FadeInOut(CardManager.Inst.InitCorutine(), TurnManager.Inst.StartGameCorutine()));
                break;
            case FIELD_TYPE.EVENT:
                StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.SHOP:
                StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.REST:
                StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.MAP:
                StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
        }
    }
}
