using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] TalkWindow talkWindow;
    public void ShowTalk()
    {
        talkWindow.GetComponent<SpriteRenderer>().DOFade(1, 1).OnComplete(() =>
        {
            Debug.Log("설명창 보이기");
            talkWindow.ShowTalk();
        });
    }
    public void HideTalk()
    {
        talkWindow.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() =>
        {
            Debug.Log("설명창 끄기");
            GhostManager.Inst.HideGhost();
        });
    }
}
