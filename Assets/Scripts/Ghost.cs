using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] TalkWindow talkWindow;
    public IEnumerator ShowTalk()
    {
        this.talkWindow.gameObject.SetActive(true);
        yield return this.StartCoroutine(TalkWindow.Inst.ShowTalkWindowCoroutine());
    }
    public IEnumerator HideTalk()
    {
        this.talkWindow.gameObject.SetActive(false);
        yield return this.StartCoroutine(GhostManager.Inst.HideGhost());
    }
}
