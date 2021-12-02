using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TOPBAR_TYPE
{
    Hp,
    Question,
    CardPiece,
    SceneName,
    Bag,
    Setting,
    Skill
}

public class TopBarManager : Singleton<TopBarManager>
{
    public List<TopBarIcon> icons;

    public GameObject[] explanObj;

    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);
    }

    private void Start()
    {
        UpdateText(TOPBAR_TYPE.Hp);
        UpdateText(TOPBAR_TYPE.CardPiece);
        UpdateText(TOPBAR_TYPE.Question);
        UpdateText(TOPBAR_TYPE.SceneName);
    }

    public TMP_Text hpTMP;
    public TMP_Text cardPieceTMP;
    public TMP_Text questionTMP;
    public TMP_Text sceneNameTMP;

    public void InitPosition()
    {
        transform.position = new Vector3(0, 4.73f, -5);
    }

    public void UpdateText(TOPBAR_TYPE type)
    {
        switch (type)
        {
            case TOPBAR_TYPE.Hp:
                hpTMP.text = PlayerManager.Inst.HpString;
                break;
            case TOPBAR_TYPE.Question:
                questionTMP.text = PlayerManager.Inst.QuestionCard.ToString();
                break;
            case TOPBAR_TYPE.CardPiece:
                cardPieceTMP.text = PlayerManager.Inst.CardPiece.ToString();
                break;
            case TOPBAR_TYPE.SceneName:
                sceneNameTMP.text = MapManager.CurrentSceneName;
                break;
            case TOPBAR_TYPE.Bag:
                break;
            case TOPBAR_TYPE.Setting:
                break;
            case TOPBAR_TYPE.Skill:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void Open(TopBarIcon icon)
    {
        if (icon.IsLock)
        {
            return;
        }

        switch (icon.type)
        {
            case TOPBAR_TYPE.Bag:
                BagManager.Inst.Open();
                break;
            case TOPBAR_TYPE.Setting:
                SettingManager.Inst.Open();
                break;
            case TOPBAR_TYPE.Skill:
                SkillManager.Inst.Open();
                break;
            case TOPBAR_TYPE.Hp:
                break;
            case TOPBAR_TYPE.Question:
                break;
            case TOPBAR_TYPE.CardPiece:
                break;
            case TOPBAR_TYPE.SceneName:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Open(TOPBAR_TYPE type)
    {
        if (GetIcon(type).IsLock)
        {
            return;
        }

        switch (type)
        {
            case TOPBAR_TYPE.Bag:
                BagManager.Inst.Open();
                break;
            case TOPBAR_TYPE.Setting:
                SettingManager.Inst.Open();
                break;
            case TOPBAR_TYPE.Skill:
                SkillManager.Inst.Open();
                break;
            case TOPBAR_TYPE.Hp:
                break;
            case TOPBAR_TYPE.Question:
                break;
            case TOPBAR_TYPE.CardPiece:
                break;
            case TOPBAR_TYPE.SceneName:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public TopBarIcon GetIcon(TOPBAR_TYPE type)
    {
        foreach (TopBarIcon icon in icons)
        {
            if (icon.type == type)
            {
                return icon;
            }
        }

        return icons[0];
    }

    public void OnMouseEnterIcon(TOPBAR_TYPE type)
    {
        switch (type)
        {
            case TOPBAR_TYPE.Bag:
                explanObj[1].SetActive(true);
                break;
            case TOPBAR_TYPE.Setting:
                explanObj[2].SetActive(true);
                break;
            case TOPBAR_TYPE.Skill:
                explanObj[0].SetActive(true);
                break;
            case TOPBAR_TYPE.Hp:
                break;
            case TOPBAR_TYPE.Question:
                break;
            case TOPBAR_TYPE.CardPiece:
                break;
            case TOPBAR_TYPE.SceneName:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void OnMouseExitIcon(TOPBAR_TYPE type)
    {
        switch (type)
        {
            case TOPBAR_TYPE.Bag:
                explanObj[1].SetActive(false);
                break;
            case TOPBAR_TYPE.Setting:
                explanObj[2].SetActive(false);
                break;
            case TOPBAR_TYPE.Skill:
                explanObj[0].SetActive(false);
                break;
            case TOPBAR_TYPE.Hp:
                break;
            case TOPBAR_TYPE.Question:
                break;
            case TOPBAR_TYPE.CardPiece:
                break;
            case TOPBAR_TYPE.SceneName:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
