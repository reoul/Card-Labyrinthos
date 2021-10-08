using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    [SerializeField] Notification notificationPanel;

    [SerializeField] SpriteRenderer fadeInOutObj;

    void Awake()
    {
        Inst = this;
    }

    //public GameObject aaaa;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif

        //if(TouchManager.inst.IsTouchLeft())
        //    Debug.Log(Vector2.Distance(Utils.TouchPos,aaaa.transform.position));
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            TurnManager.OnAddCard.Invoke();
        if (Input.GetKeyDown(KeyCode.W))
            TurnManager.Inst.EndTurn();
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCorutine());
    }
    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(false));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(true));
    }

    IEnumerator Fade(bool isOut)
    {
        float alpha = isOut ? 0 : 1;
        if (isOut)
            while (alpha < 1)
            {
                alpha += Time.deltaTime;
                fadeInOutObj.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
        else
            while (alpha > 0)
            {
                alpha -= Time.deltaTime;
                Debug.Log(alpha);
                fadeInOutObj.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
    }
}
