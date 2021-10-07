using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    [SerializeField] Notification notificationPanel;

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
}
