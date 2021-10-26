using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TOPBAR_TYPE { HP, QUESTION, CARDPIECE, SCENENAME, BAG, SETTING };

public class TopBar : MonoBehaviour
{
    public static TopBar Inst = null;
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
        UpdateText(TOPBAR_TYPE.HP);
        UpdateText(TOPBAR_TYPE.CARDPIECE);
        UpdateText(TOPBAR_TYPE.QUESTION);
        UpdateText(TOPBAR_TYPE.SCENENAME);
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
                hpTMP.text = PlayerManager.Inst.hpString;
                break;
            case TOPBAR_TYPE.QUESTION:
                questionTMP.text = PlayerManager.Inst.question_card.ToString();
                break;
            case TOPBAR_TYPE.CARDPIECE:
                cardPieceTMP.text = PlayerManager.Inst.card_piece.ToString();
                break;
            case TOPBAR_TYPE.SCENENAME:
                sceneNameTMP.text = MapManager.Inst.CurrentSceneName;
                break;
        }
    }

    public void Click(TopBarIcon icon)
    {
        switch (icon.type)
        {
            case TOPBAR_TYPE.BAG:
                BagManager.Inst.Open();
                break;
            case TOPBAR_TYPE.SETTING:
                break;
        }
    }
}
