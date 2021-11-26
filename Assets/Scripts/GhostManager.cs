using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Inst;
    [SerializeField] private Ghost ghost;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //ShowGhost();
    }

    public IEnumerator ShowGhost()
    {
        //Tween tween = this.ghost.GetComponent<SpriteRenderer>().DOFade(1, 1);
        Tween tween = ghost.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        yield return tween.WaitForCompletion();
        yield return StartCoroutine(ghost.ShowTalk());
    }
    public IEnumerator HideGhost()
    {
        //Tween tween = this.ghost.GetComponent<SpriteRenderer>().DOFade(0, 1);
        Tween tween = ghost.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        yield return tween.WaitForCompletion();
    }

    public void MoveOriginPos()
    {
        transform.position = new Vector3(-7.39f, 2.85f, -5);
    }

    public void MoveTutorialPos()
    {
        transform.position = new Vector3(-7.95f, -2.88f, -5);
    }
    public void MoveTutorialSkillPos()
    {
        transform.position = new Vector3(-7.59f, -3.87f, -5);
    }
}
