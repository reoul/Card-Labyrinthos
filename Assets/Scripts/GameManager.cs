using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    [SerializeField] private Notification notificationPanel;
    [SerializeField] private GameObject endingCredit;
    [SerializeField] private GameObject gameOver;

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

    private void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ResetManager.Inst.ResetGame();
        }

        if (Input.GetKeyDown(KeyCode.I) && TopBar.Inst != null)
        {
            TopBar.Inst.Open(TOPBAR_TYPE.BAG);
        }

        if (Input.GetKeyDown(KeyCode.K) && TopBar.Inst.GetIcon(TOPBAR_TYPE.SKILL).gameObject.activeInHierarchy &&
            TopBar.Inst != null)
        {
            TopBar.Inst.Open(TOPBAR_TYPE.SKILL);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && TopBar.Inst != null)
        {
            TopBar.Inst.Open(TOPBAR_TYPE.SETTING);
        }
    }

    public void Notification(string message)
    {
        if (notificationPanel == null)
        {
            notificationPanel = GameObject.Find("MyTurn").GetComponent<Notification>();
        }

        if (notificationPanel != null)
        {
            notificationPanel.Show(message);
        }
    }

    public void CloseAllUI()
    {
        BagManager.Inst.Close();
        SkillManager.Inst.Close();
    }

    public void Ending()
    {
        StartCoroutine(EndingCoroutine());
    }

    public IEnumerator EndingCoroutine()
    {
        yield return StartCoroutine(GhostManager.Inst.ShowGhost());
        for (int i = 0; i < TalkWindow.Inst.talks[14].Count; i++)
        {
            yield return StartCoroutine(TalkWindow.Inst.TalkTypingCoroutine(14, i));
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagIndexCoroutine());
            yield return StartCoroutine(TalkWindow.Inst.CheckFlagNextCoroutine());
        }

        yield return StartCoroutine(TalkWindow.Inst.HideText());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(EndingCreditCoroutine());
    }

    private IEnumerator EndingCreditCoroutine()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.ENDING);
        endingCredit.SetActive(true);
        yield return new WaitForSeconds(5f);
        ResetManager.Inst.ResetGame();
    }

    public void GameOver()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.ENDING);
        gameOver.SetActive(true);
    }
}
