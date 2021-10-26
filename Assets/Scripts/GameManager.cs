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
