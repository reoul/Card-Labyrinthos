using UnityEngine;

[System.Serializable]
public class FieldData
{
    [SerializeField]
    FIELD_TYPE field_type;
    [SerializeField]
    EVENT_TYPE event_type;
    [SerializeField]
    MONSTER_TYPE monster_type;

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

public enum FIELD_TYPE { MONSTER, EVENT, REST }
public enum EVENT_TYPE { EVENT1, EVENT2, EVENT3 };
public enum MONSTER_TYPE { MONSTER1, MONSTER2, MONSTER3 };


public class MapManager : MonoBehaviour
{
    public static MapManager Inst { get; private set; }

    [SerializeField]
    bool isTrue;
    [SerializeField]
    int a;
    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void IconMouseUp(FieldData fieldData)
    {
        FadeManager.Inst.FadeOut();
    }
}
