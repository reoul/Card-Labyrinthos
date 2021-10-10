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
        FadeManager.FadeEvent += GetLoadMethod();
        StartCoroutine(FadeManager.Inst.FadeInOut(TurnManager.Inst.StartGameCorutine()));
    }

    public void LoadBattleScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene("Battle");
    }
    public void LoadRestScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene("Rest");
    }
    public void LoadEventScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene("Event");
    }
    public void LoadShopScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene("Shop");
    }
    public void LoadMapScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene("Map");
    }

    public EventHandler GetLoadMethod()
    {
        switch (data.field_type.ToString())
        {
            case "BATTLE":
                return new EventHandler(LoadBattleScene);
            case "SHOP":
                return new EventHandler(LoadShopScene);
            case "EVENT":
                return new EventHandler(LoadEventScene);
            case "REST":
                return new EventHandler(LoadRestScene);
            default:
                return new EventHandler(LoadMapScene);
        }
    }
}
