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
        if (Input.GetKeyDown(KeyCode.I))
            BagManager.Inst.Open();
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
}
