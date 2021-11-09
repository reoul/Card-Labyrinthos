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

    [SerializeField] GameObject hitObj;

    void Awake()
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

        if (Input.GetKeyDown(KeyCode.P))
            if (EnemyManager.Inst.enemys.Count > 0)
                EnemyManager.Inst.enemys[0].Damage(EnemyManager.Inst.enemys[0].hpbar.hp - 1);

        if (Input.GetKeyDown(KeyCode.I))
            TopBar.Inst.Open(TOPBAR_TYPE.BAG);

        if (Input.GetKeyDown(KeyCode.K) && TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).gameObject.activeInHierarchy)
            TopBar.Inst.Open(TOPBAR_TYPE.SKILL);

        if (Input.GetKeyDown(KeyCode.Escape))
            TopBar.Inst.Open(TOPBAR_TYPE.SETTING);
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCoroutine());
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
