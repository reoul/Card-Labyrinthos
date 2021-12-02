using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GhostManager : Singleton<GhostManager>
{
    [SerializeField] private Ghost ghost;

    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);
    }

    public IEnumerator ShowGhost()
    {
        Tween tween = ghost.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        yield return tween.WaitForCompletion();
        yield return StartCoroutine(ghost.ShowTalk());
    }

    public IEnumerator HideGhost()
    {
        Tween tween = ghost.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        yield return tween.WaitForCompletion();
    }

    public void MoveOriginPos()
    {
        transform.position = new Vector3(-7.39f, 2.85f, -5);
    }

    public void MoveTutorialSkillPos()
    {
        transform.position = new Vector3(-7.59f, -3.87f, -5);
    }
}
