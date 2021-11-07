using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Inst = null;
    [SerializeField] Ghost ghost;

    private void Awake()
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

    private void Start()
    {
        //ShowGhost();
    }

    public IEnumerator ShowGhost()
    {
        Tween tween = this.ghost.GetComponent<SpriteRenderer>().DOFade(1, 1);
        yield return tween.WaitForCompletion();
        yield return StartCoroutine(ghost.ShowTalk());
    }
    public IEnumerator HideGhost()
    {
        Tween tween = this.ghost.GetComponent<SpriteRenderer>().DOFade(0, 1);
        yield return tween.WaitForCompletion();
    }
}
