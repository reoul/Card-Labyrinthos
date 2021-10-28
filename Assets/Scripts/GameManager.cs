using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    [SerializeField] Notification notificationPanel;

    [SerializeField] SpriteRenderer fadeInOutObj;

    void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //StartGame();
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CardManager.Inst.SelectCardNumAdd(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            CardManager.Inst.SelectCardNumAdd(-1);
        //if (Input.GetKeyDown(KeyCode.T))
        //    ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.CARDBACK, Vector3.zero, new Vector3(5, 0), null, 0.5f);
        if (Input.GetKeyDown(KeyCode.I))
            BagManager.Inst.Open();
        if (Input.GetKeyDown(KeyCode.K))
            SkillManager.Inst.Open();
        if (Input.GetKeyDown(KeyCode.P))
            if (EnemyManager.Inst.enemys.Count > 0)
                EnemyManager.Inst.enemys[0].Damage(EnemyManager.Inst.enemys[0].hpbar.hp - 1);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCorutine());
    }
    public void Notification(string message)
    {
        if (notificationPanel == null)
            notificationPanel = GameObject.Find("MyTurn").GetComponent<Notification>();
        if (notificationPanel != null)
            notificationPanel.Show(message);
    }

    public void CloseAllUI()
    {
        BagManager.Inst.Close();
        SkillManager.Inst.Close();
    }
}
