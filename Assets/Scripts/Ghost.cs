using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] TalkWindow talkWindow;
    public void ShowTalk()
    {
        talkWindow.gameObject.SetActive(true);
        talkWindow.GetComponent<SpriteRenderer>().DOFade(1, 1).OnComplete(() =>
        {
            talkWindow.ShowTalk();
        });
    }
    public void HideTalk()
    {
        talkWindow.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() =>
        {
            talkWindow.gameObject.SetActive(false);
            GhostManager.Inst.HideGhost();
        });
    }
}
