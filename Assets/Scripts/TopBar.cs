using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TOPBAR_TYPE { HP, QUESTION, CARDPIECE, SCENENAME, BAG, SETTING, SKILL }

public class TopBar : MonoBehaviour
{
    public static TopBar Inst;

    public List<TopBarIcon> icons;

    public GameObject[] explanObj;

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

    private void Start()
    {
        this.UpdateText(TOPBAR_TYPE.HP);
        this.UpdateText(TOPBAR_TYPE.CARDPIECE);
        this.UpdateText(TOPBAR_TYPE.QUESTION);
        this.UpdateText(TOPBAR_TYPE.SCENENAME);
    }

    public TMP_Text hpTMP;
    public TMP_Text cardPieceTMP;
    public TMP_Text questionTMP;
    public TMP_Text sceneNameTMP;

    public void InitPosition()
    {
        this.transform.position = new Vector3(0, 4.73f, -5);
    }

    public void UpdateText(TOPBAR_TYPE type)
    {
        switch (type)
        {
            case TOPBAR_TYPE.HP:
                this.hpTMP.text = PlayerManager.Inst.hpString;
                break;
            case TOPBAR_TYPE.QUESTION:
                this.questionTMP.text = PlayerManager.Inst.question_card.ToString();
                break;
            case TOPBAR_TYPE.CARDPIECE:
                this.cardPieceTMP.text = PlayerManager.Inst.card_piece.ToString();
                break;
            case TOPBAR_TYPE.SCENENAME:
                this.sceneNameTMP.text = MapManager.Inst.CurrentSceneName;
                break;
        }
    }

    public void Open(TopBarIcon icon)
    {
        if (icon.isLock)
            return;
        switch (icon.type)
        {
            case TOPBAR_TYPE.BAG:
                BagManager.Inst.Open();
                break;
            case TOPBAR_TYPE.SETTING:
                SettingManager.Inst.Open();
                break;
            case TOPBAR_TYPE.SKILL:
                SkillManager.Inst.Open();
                break;
        }
    }
    public void Open(TOPBAR_TYPE type)
    {
        if (this.GetIcon(type).isLock)
            return;
        switch (type)
        {
            case TOPBAR_TYPE.BAG:
                BagManager.Inst.Open();
                break;
            case TOPBAR_TYPE.SETTING:
                SettingManager.Inst.Open();
                break;
            case TOPBAR_TYPE.SKILL:
                SkillManager.Inst.Open();
                break;
        }
    }

    public TopBarIcon GetIcon(TOPBAR_TYPE type)
    {
        for (int i = 0; i < this.icons.Count; i++)
        {
            if (this.icons[i].type == type)
                return this.icons[i];
        }
        return this.icons[0];
    }

    public void OnMouseEnterIcon(TOPBAR_TYPE type)
    {
        switch (type)
        {
            case TOPBAR_TYPE.BAG:
                this.explanObj[1].SetActive(true);
                break;
            case TOPBAR_TYPE.SETTING:
                this.explanObj[2].SetActive(true);
                break;
            case TOPBAR_TYPE.SKILL:
                this.explanObj[0].SetActive(true);
                break;
        }
    }
    public void OnMouseExitIcon(TOPBAR_TYPE type)
    {
        switch (type)
        {
            case TOPBAR_TYPE.BAG:
                this.explanObj[1].SetActive(false);
                break;
            case TOPBAR_TYPE.SETTING:
                this.explanObj[2].SetActive(false);
                break;
            case TOPBAR_TYPE.SKILL:
                this.explanObj[0].SetActive(false);
                break;
        }
    }
}
