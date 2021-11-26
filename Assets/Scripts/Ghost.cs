using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private TalkWindow talkWindow;
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
