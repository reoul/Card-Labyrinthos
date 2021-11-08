using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] TalkWindow talkWindow;
    public IEnumerator ShowTalk()
    {
        talkWindow.gameObject.SetActive(true);
        yield return StartCoroutine(TalkWindow.Inst.ShowTalkWindowCoroutine());
    }
    public IEnumerator HideTalk()
    {
        talkWindow.gameObject.SetActive(false);
        yield return StartCoroutine(GhostManager.Inst.HideGhost());
    }
}
