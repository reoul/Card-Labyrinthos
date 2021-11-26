using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
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
public enum EVENT_TYPE { EVENT1, EVENT2, EVENT3 }

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
    public static MapManager Inst;

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

    public bool isFinishToturialBattle;
    public bool isFinishToturialBag;
    public bool isFinishToturialDebuff;
    public bool isFinishToturialRest;
    public bool isFinishToturialEvent;
    public bool isFinishToturialShop;
    public bool isFinishToturialBoss;

    public bool isTutorialReadyBag;
    public bool isTutorialReadyDebuff;
    public bool isTutorialReadyRest;
    public bool isTutorialReadyEvent;
    public bool isTutorialReadyShop;
    public bool isTutorialReadyBoss;

    public bool isTutorialOpenBag;
    public bool isTutorialInRest;
    public bool isTutorialInEvent;
    public bool isTutorialInShop;
    public bool isTutorialBoss;

    bool isMoveCamera = false;
    Vector3 lastMousePos;

    public int tutorialIndex;
    bool isBattleDebuffOff;
    public int lastField;
    public bool isFinishTutorialEventField;

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

    public void IconMouseUp(Field field)
    {
        SoundManager.Inst.Play(MAPSOUND.CHOICE_FIELD);
        this.fieldData = field.fieldData;
        this.isBattleDebuffOff = field.isDebuffOff;
        for (int i = 0; i < this.fields.Length; i++)
        {
            if (this.fields[i].Equals(field))
            {
                this.selectFieldIndex = i;
            }
        }
        FadeManager.FadeEvent += this.LoadScene;
        this.StartEvent();
    }

    public void LoadScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene(this.fieldData.field_type.ToString());
    }

    public void LoadMapScene(bool clear)
    {
        if (this.isClear.Length != 0) this.isClear[this.selectFieldIndex] = clear;
        if (clear) this.lastField = this.selectFieldIndex;
        TurnManager.Inst.isFinish = false;
        this.CheckTutorialReady();
        this.fieldData.field_type = FIELD_TYPE.MAP;
        TalkWindow.Inst.SetFlagIndex(false);
        TalkWindow.Inst.SetFlagNext(false);
        TalkWindow.Inst.SetSkip(false);
        this.isTutorialOpenBag = false;
        FadeManager.FadeEvent += this.LoadScene;
        this.StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null, this.FieldClearCheckCoroutine(), this.InitSkillTime(), null, this.GetTutorialCoroutine()));
    }

    public void LoadTutorialScene()
    {
        this.fieldData.field_type = FIELD_TYPE.TUTORIAL;
        FadeManager.FadeEvent += this.LoadScene;
        this.StartCoroutine(FadeManager.Inst.FadeInOut());
    }

    void StartEvent()
    {
        switch (this.fieldData.field_type)
        {
            case FIELD_TYPE.BATTLE:
                this.StartCoroutine(FadeManager.Inst.FadeInOut(TurnManager.Inst.ShowDebuffCoroutine(), null, null,
                    PlayerManager.Inst.SetupGameCoroutine(), null, null,
                        CardManager.Inst.InitCoroutine(), TurnManager.Inst.StartGameCoroutine()));
                break;
            case FIELD_TYPE.EVENT:
                this.StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    EventManager.Inst.RandomEventCoroutine(), null, null,
                        CardManager.Inst.InitCoroutine(), TurnManager.Inst.StartGameCoroutine()));
                break;
            case FIELD_TYPE.SHOP:
                this.StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.REST:
                this.StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.MAP:
                this.StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null, this.FieldClearCheckCoroutine(), this.InitSkillTime(), null, this.GetTutorialCoroutine()));
                break;
            case FIELD_TYPE.BOSS:
                this.StartCoroutine(FadeManager.Inst.FadeInOut(TurnManager.Inst.ShowDebuffCoroutine(), null, null,
                    PlayerManager.Inst.SetupGameCoroutine(), null, null,
                        CardManager.Inst.InitCoroutine(), TurnManager.Inst.StartGameCoroutine()));
                break;
            case FIELD_TYPE.TUTORIAL:
                this.StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    null, null, null,
                        TutorialManager.Inst.TutorialCoroutine()));
                break;
            case FIELD_TYPE.TUTORIAL2:
                this.StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    PlayerManager.Inst.SetupGameCoroutine(), null, null, this.tutorialIndex == 1 ? CardManager.Inst.FixedCardNumToturial2Coroutine() : null, CardManager.Inst.InitCoroutine(), TurnManager.Inst.StartGameCoroutine()));
                break;
        }
    }

    public IEnumerator FieldClearCheckCoroutine()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.MAP);
        if (this.fieldParent == null) this.fieldParent = GameObject.Find("FieldParent");
        this.fields = this.fieldParent.GetComponentsInChildren<Field>(true);
        if (this.isClear.Length == 0) this.isClear = new bool[this.fields.Length];
        this.isClear[0] = true;

        for (int i = 0; i < this.fields.Length; i++)
        {
            this.fields[i].isClear = this.isClear[i];
            for (int j = 0; j < this.fields.Length; j++)
            {
                if (Vector3.Distance(this.fields[i].transform.position, this.fields[j].transform.position) < 2.5f &&
                        Vector3.Distance(this.fields[i].transform.position, this.fields[j].transform.position) >= 1.8f)
                    this.fields[i].surroundingObj.Add(this.fields[j].gameObject);
            }

            this.fields[i].UpdateClearImage();
        }
        Vector3 pos = Map.Inst.transform.parent.transform.position - this.fields[this.lastField].transform.position;
        Map.Inst.MoveMap(new Vector3(pos.x, pos.y, 0));
        yield return null;
    }

    public void CheckTutorialReady()
    {
        for (int i = this.isClear.Length - 1; i >= 0; i--)
        {
            if (this.isClear[i])
            {
                if (i == 1)     //가방
                {
                    if (this.isFinishToturialBattle)
                    {
                        this.isTutorialReadyBag = true;
                        break;
                    }
                }
                else if (i == 2)    //저주
                {
                    if (this.isFinishToturialBag)
                    {
                        this.isTutorialReadyDebuff = true;
                        break;
                    }
                }
                else if (i == 3)    //휴식방
                {
                    if (this.isFinishToturialDebuff)
                    {
                        this.isTutorialReadyRest = true;
                        break;
                    }
                }
                else if (i == 4)    //이벤트
                {
                    if (this.isFinishToturialRest)
                    {
                        this.isTutorialReadyEvent = true;
                        break;
                    }
                }
                else if (i == 5)    //상점
                {
                    if (this.isFinishToturialEvent)
                    {
                        this.isTutorialReadyShop = true;
                        break;
                    }
                }
                else if (i == 10)    //보스
                {
                    if (this.isFinishToturialShop)
                    {
                        this.isTutorialReadyBoss = true;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }

    public IEnumerator InitSkillTime()
    {
        SkillManager.Inst.InitSkillTime();
        yield return null;
    }

    IEnumerator GetTutorialCoroutine()
    {
        if (!this.isFinishToturialBattle)
            return this.MapTutorialBattleCoroutine();
        if (this.isTutorialReadyBag && !this.isFinishToturialBag)
            return this.MapTutorialBagCoroutine();
        if (this.isTutorialReadyDebuff && !this.isFinishToturialDebuff)
            return this.MapTutorialDebuffCoroutine();
        if (this.isTutorialReadyRest && !this.isFinishToturialRest)
            return this.MapTutorialRestCoroutine();
        if (this.isTutorialReadyEvent && !this.isFinishToturialEvent)
        {
            this.isFinishToturialEvent = true;
            return this.MapTutorialEventCoroutine();
        }

        if (this.isTutorialReadyShop && !this.isFinishToturialShop)
            return this.MapTutorialShopCoroutine();
        if (this.isTutorialReadyBoss && !this.isFinishToturialBoss)
        {
            this.isFinishToturialBoss = true;
            return this.MapTutorialBossCoroutine();
        }
        return null;
    }

    public IEnumerator MapTutorialBattleCoroutine()
    {
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[1].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, 0, 0), ArrowCreateDirection.RIGHT);
            }
            else if (i == 2)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(this.fields[0].transform.position + -Vector3.right, ArrowCreateDirection.LEFT, this.fields[0].transform);
            }
            else if (i == 3)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(this.fields[1].transform.position + -Vector3.right, ArrowCreateDirection.LEFT, this.fields[1].transform);
            }
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(1, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }

        this.isFinishToturialBattle = true;
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        yield return null;
    }
    public IEnumerator MapTutorialBagCoroutine()       //가방 설명
    {
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[3].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(TopBar.Inst.GetIcon(TOPBAR_TYPE.BAG).transform.position + new Vector3(0, -1, 0), ArrowCreateDirection.DOWN);
            }
            else if (i == 1)
            {
                ArrowManager.Inst.DestoryAllArrow();
            }
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(3, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
            while (!this.isTutorialOpenBag)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        this.isFinishToturialBag = true;
        yield return null;
    }

    public IEnumerator MapTutorialDebuffCoroutine()       //Debuff 설명
    {
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[5].Count; i++)
        {
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(5, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        this.isFinishToturialDebuff = true;
        TurnManager.Inst.isTutorialDebuffBar = true;
        yield return null;
    }
    public IEnumerator MapTutorialRestCoroutine()
    {
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[7].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(this.fields[4].transform.position + Vector3.right, ArrowCreateDirection.RIGHT, this.fields[4].transform);
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(7, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        this.isFinishToturialRest = true;
        yield return null;
    }
    public IEnumerator MapTutorialEventCoroutine()
    {
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[9].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(this.fields[5].transform.position + Vector3.right, ArrowCreateDirection.RIGHT, this.fields[5].transform);
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(9, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        this.isFinishToturialEvent = true;
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        this.isFinishTutorialEventField = true;
        yield return null;
    }
    public IEnumerator MapTutorialShopCoroutine()
    {
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        this.fields[7].isReady = false;
        for (int i = 0; i < TalkWindow.Inst.talks[11].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(this.fields[6].transform.position + Vector3.right, ArrowCreateDirection.RIGHT, this.fields[6].transform);
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(11, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        this.isFinishToturialShop = true;
        yield return null;
    }
    public IEnumerator MapTutorialBossCoroutine()
    {
        this.fields[6].isReady = false;
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[13].Count; i++)
        {
            if (i == 0)
                ArrowManager.Inst.CreateArrowObj(this.fields[11].transform.position + Vector3.right * 2, ArrowCreateDirection.RIGHT, this.fields[11].transform);
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(13, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        this.isFinishToturialBoss = true;
        this.isTutorialBoss = true;
        this.fields[6].isReady = true;
        yield return null;
    }
}
