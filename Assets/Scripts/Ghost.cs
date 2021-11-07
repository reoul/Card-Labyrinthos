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
        talkWindow.ShowTalk();
    }
    public void HideTalk()
    {
        talkWindow.gameObject.SetActive(false);
        GhostManager.Inst.HideGhost();
    }
}
