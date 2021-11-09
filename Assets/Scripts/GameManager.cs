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

    [SerializeField] GameObject endingCredit;
    [SerializeField] GameObject gameOver;

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
        if (Input.GetKeyDown(KeyCode.F12))
            ResetManager.Inst.ResetGame();

        if (Input.GetKeyDown(KeyCode.F11))
            if (EnemyManager.Inst.enemys.Count > 0)
                EnemyManager.Inst.enemys[0].Damage(EnemyManager.Inst.enemys[0].hpbar.hp - 1);

        if (Input.GetKeyDown(KeyCode.I))
            if (TopBar.Inst != null)
                TopBar.Inst.Open(TOPBAR_TYPE.BAG);

        if (Input.GetKeyDown(KeyCode.K) && TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).gameObject.activeInHierarchy)
            if (TopBar.Inst != null)
                TopBar.Inst.Open(TOPBAR_TYPE.SKILL);

        if (Input.GetKeyDown(KeyCode.Escape))
            if (TopBar.Inst != null)
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

    public void EndingCredit()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.ENDING);
        endingCredit.SetActive(true);
        endingCredit.transform.DOMoveY(65, 30).SetEase(Ease.Linear).OnComplete(() =>
        {
            ResetManager.Inst.ResetGame();
        });
    }

    public void GameOver()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.ENDING);
        gameOver.SetActive(true);
    }
}
