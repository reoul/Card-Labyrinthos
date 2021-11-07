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
        this.ghost.GetComponent<SpriteRenderer>().DOFade(1, 1).OnComplete(() =>
        {
            ghost.ShowTalk();
        });
        yield return null;
    }
    public void HideGhost()
    {
        this.ghost.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() =>
        {
        });
    }
}
