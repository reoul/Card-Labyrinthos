using System.Collections;
using DG.Tweening;
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
        this.StartCoroutine(TurnManager.Inst.StartGameCoroutine());
    }
    public void Notification(string message)
    {
        if (this.notificationPanel == null) this.notificationPanel = GameObject.Find("MyTurn").GetComponent<Notification>();
        if (this.notificationPanel != null) this.notificationPanel.Show(message);
    }

    public void CloseAllUI()
    {
        BagManager.Inst.Close();
        SkillManager.Inst.Close();
    }

    public void Ending()
    {
        this.StartCoroutine(this.EndingCoroutine());
    }

    public IEnumerator EndingCoroutine()
    {
        yield return this.StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[14].Count; i++)
        {
            yield return this.StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(14, i));
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return this.StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }
        yield return this.StartCoroutine(TalkWindow.Inst.HideText());
        yield return new WaitForSeconds(0.5f);
        this.StartCoroutine(this.EndingCreditCoroutine());
    }

    IEnumerator EndingCreditCoroutine()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.ENDING);
        this.endingCredit.SetActive(true);
        yield return new WaitForSeconds(5f);
        ResetManager.Inst.ResetGame();
    }

    public void GameOver()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.ENDING);
        this.gameOver.SetActive(true);
    }
}
