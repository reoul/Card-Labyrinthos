﻿using System;
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public FieldData fieldData;
    [SerializeField] private int selectFieldIndex;

    [SerializeField] private bool[] isClear;     //필드 클리어 여부, 해당 필드가 끝이 날때 변경
    [SerializeField] private GameObject fieldParent;
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

    private bool isMoveCamera = false;
    private Vector3 lastMousePos;

    public int tutorialIndex;
    private bool isBattleDebuffOff;
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
        fieldData = field.fieldData;
        isBattleDebuffOff = field.isDebuffOff;
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].Equals(field))
            {
                selectFieldIndex = i;
            }
        }
        FadeManager.FadeEvent += LoadScene;
        StartEvent();
    }

    public void LoadScene(object obj, EventArgs e)
    {
        SceneManager.LoadScene(fieldData.field_type.ToString());
    }

    public void LoadMapScene(bool clear)
    {
        if (isClear.Length != 0)
        {
            isClear[selectFieldIndex] = clear;
        }

        if (clear)
        {
            lastField = selectFieldIndex;
        }

        TurnManager.Inst.isFinish = false;
        CheckTutorialReady();
        fieldData.field_type = FIELD_TYPE.MAP;
        TalkWindow.Inst.SetFlagIndex(false);
        TalkWindow.Inst.SetFlagNext(false);
        TalkWindow.Inst.SetSkip(false);
        isTutorialOpenBag = false;
        FadeManager.FadeEvent += LoadScene;
        StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null, FieldClearCheckCoroutine(), InitSkillTime(), null, GetTutorialCoroutine()));
    }

    public void LoadTutorialScene()
    {
        fieldData.field_type = FIELD_TYPE.TUTORIAL;
        FadeManager.FadeEvent += LoadScene;
        StartCoroutine(FadeManager.Inst.FadeInOut());
    }

    private void StartEvent()
    {
        switch (fieldData.field_type)
        {
            case FIELD_TYPE.BATTLE:
                StartCoroutine(FadeManager.Inst.FadeInOut(TurnManager.Inst.ShowDebuffCoroutine(), null, null,
                    PlayerManager.Inst.SetupGameCoroutine(), null, null,
                        CardManager.Inst.InitCoroutine(), TurnManager.Inst.StartGameCoroutine()));
                break;
            case FIELD_TYPE.EVENT:
                StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    EventManager.Inst.RandomEventCoroutine(), null, null,
                        CardManager.Inst.InitCoroutine(), TurnManager.Inst.StartGameCoroutine()));
                break;
            case FIELD_TYPE.SHOP:
                StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.REST:
                StartCoroutine(FadeManager.Inst.FadeInOut());
                break;
            case FIELD_TYPE.MAP:
                StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null, FieldClearCheckCoroutine(), InitSkillTime(), null, GetTutorialCoroutine()));
                break;
            case FIELD_TYPE.BOSS:
                StartCoroutine(FadeManager.Inst.FadeInOut(TurnManager.Inst.ShowDebuffCoroutine(), null, null,
                    PlayerManager.Inst.SetupGameCoroutine(), null, null,
                        CardManager.Inst.InitCoroutine(), TurnManager.Inst.StartGameCoroutine()));
                break;
            case FIELD_TYPE.TUTORIAL:
                StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    null, null, null,
                        TutorialManager.Inst.TutorialCoroutine()));
                break;
            case FIELD_TYPE.TUTORIAL2:
                StartCoroutine(FadeManager.Inst.FadeInOut(null, null, null,
                    PlayerManager.Inst.SetupGameCoroutine(), null, null, tutorialIndex == 1 ? CardManager.Inst.FixedCardNumToturial2Coroutine() : null, CardManager.Inst.InitCoroutine(), TurnManager.Inst.StartGameCoroutine()));
                break;
        }
    }

    public IEnumerator FieldClearCheckCoroutine()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.MAP);
        if (fieldParent == null)
        {
            fieldParent = GameObject.Find("FieldParent");
        }

        fields = fieldParent.GetComponentsInChildren<Field>(true);
        if (isClear.Length == 0)
        {
            isClear = new bool[fields.Length];
        }

        isClear[0] = true;

        for (int i = 0; i < fields.Length; i++)
        {
            fields[i].isClear = isClear[i];
            for (int j = 0; j < fields.Length; j++)
            {
                if (Vector3.Distance(fields[i].transform.position, fields[j].transform.position) < 2.5f &&
                        Vector3.Distance(fields[i].transform.position, fields[j].transform.position) >= 1.8f)
                {
                    fields[i].surroundingObj.Add(fields[j].gameObject);
                }
            }

            fields[i].UpdateClearImage();
        }
        Vector3 pos = Map.Inst.transform.parent.transform.position - fields[lastField].transform.position;
        Map.Inst.MoveMap(new Vector3(pos.x, pos.y, 0));
        yield return null;
    }

    public void CheckTutorialReady()
    {
        for (int i = isClear.Length - 1; i >= 0; i--)
        {
            if (isClear[i])
            {
                if (i == 1)     //가방
                {
                    if (isFinishToturialBattle)
                    {
                        isTutorialReadyBag = true;
                        break;
                    }
                }
                else if (i == 2)    //저주
                {
                    if (isFinishToturialBag)
                    {
                        isTutorialReadyDebuff = true;
                        break;
                    }
                }
                else if (i == 3)    //휴식방
                {
                    if (isFinishToturialDebuff)
                    {
                        isTutorialReadyRest = true;
                        break;
                    }
                }
                else if (i == 4)    //이벤트
                {
                    if (isFinishToturialRest)
                    {
                        isTutorialReadyEvent = true;
                        break;
                    }
                }
                else if (i == 5)    //상점
                {
                    if (isFinishToturialEvent)
                    {
                        isTutorialReadyShop = true;
                        break;
                    }
                }
                else if (i == 10)    //보스
                {
                    if (isFinishToturialShop)
                    {
                        isTutorialReadyBoss = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

    public IEnumerator InitSkillTime()
    {
        SkillManager.Inst.InitSkillTime();
        yield return null;
    }

    private IEnumerator GetTutorialCoroutine()
    {
        if (!isFinishToturialBattle)
        {
            return MapTutorialBattleCoroutine();
        }

        if (isTutorialReadyBag && !isFinishToturialBag)
        {
            return MapTutorialBagCoroutine();
        }

        if (isTutorialReadyDebuff && !isFinishToturialDebuff)
        {
            return MapTutorialDebuffCoroutine();
        }

        if (isTutorialReadyRest && !isFinishToturialRest)
        {
            return MapTutorialRestCoroutine();
        }

        if (isTutorialReadyEvent && !isFinishToturialEvent)
        {
            isFinishToturialEvent = true;
            return MapTutorialEventCoroutine();
        }

        if (isTutorialReadyShop && !isFinishToturialShop)
        {
            return MapTutorialShopCoroutine();
        }

        if (isTutorialReadyBoss && !isFinishToturialBoss)
        {
            isFinishToturialBoss = true;
            return MapTutorialBossCoroutine();
        }
        return null;
    }

    public IEnumerator MapTutorialBattleCoroutine()
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[1].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(TalkWindow.Inst.transform.position + new Vector3(3, 0, 0), ArrowCreateDirection.Right);
            }
            else if (i == 2)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(fields[0].transform.position + -Vector3.right, ArrowCreateDirection.Left, fields[0].transform);
            }
            else if (i == 3)
            {
                ArrowManager.Inst.DestoryAllArrow();
                ArrowManager.Inst.CreateArrowObj(fields[1].transform.position + -Vector3.right, ArrowCreateDirection.Left, fields[1].transform);
            }
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(1, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }

        isFinishToturialBattle = true;
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        yield return null;
    }
    public IEnumerator MapTutorialBagCoroutine()       //가방 설명
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[3].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(TopBar.Inst.GetIcon(TOPBAR_TYPE.BAG).transform.position + new Vector3(0, -1, 0), ArrowCreateDirection.Down);
            }
            else if (i == 1)
            {
                ArrowManager.Inst.DestoryAllArrow();
            }
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(3, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
            while (!isTutorialOpenBag)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishToturialBag = true;
        yield return null;
    }

    public IEnumerator MapTutorialDebuffCoroutine()       //Debuff 설명
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[5].Count; i++)
        {
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(5, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishToturialDebuff = true;
        TurnManager.Inst.isTutorialDebuffBar = true;
        yield return null;
    }
    public IEnumerator MapTutorialRestCoroutine()
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[7].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(fields[4].transform.position + Vector3.right, ArrowCreateDirection.Right, fields[4].transform);
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(7, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishToturialRest = true;
        yield return null;
    }
    public IEnumerator MapTutorialEventCoroutine()
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[9].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(fields[5].transform.position + Vector3.right, ArrowCreateDirection.Right, fields[5].transform);
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(9, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        isFinishToturialEvent = true;
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishTutorialEventField = true;
        yield return null;
    }
    public IEnumerator MapTutorialShopCoroutine()
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        fields[7].isReady = false;
        for (int i = 0; i < TalkWindow.Inst.talks[11].Count; i++)
        {
            ArrowManager.Inst.CreateArrowObj(fields[6].transform.position + Vector3.right, ArrowCreateDirection.Right, fields[6].transform);
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(11, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishToturialShop = true;
        yield return null;
    }
    public IEnumerator MapTutorialBossCoroutine()
    {
        fields[6].isReady = false;
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[13].Count; i++)
        {
            if (i == 0)
            {
                ArrowManager.Inst.CreateArrowObj(fields[11].transform.position + Vector3.right * 2, ArrowCreateDirection.Right, fields[11].transform);
            }

            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(13, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        ArrowManager.Inst.DestoryAllArrow();
        yield return StartCoroutine(TalkWindow.Inst.HideText());
        isFinishToturialBoss = true;
        isTutorialBoss = true;
        fields[6].isReady = true;
        yield return null;
    }
}
