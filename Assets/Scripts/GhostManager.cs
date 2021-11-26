using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Inst;
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
        //Tween tween = this.ghost.GetComponent<SpriteRenderer>().DOFade(1, 1);
        Tween tween = this.ghost.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        yield return tween.WaitForCompletion();
        yield return this.StartCoroutine(this.ghost.ShowTalk());
    }
    public IEnumerator HideGhost()
    {
        //Tween tween = this.ghost.GetComponent<SpriteRenderer>().DOFade(0, 1);
        Tween tween = this.ghost.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        yield return tween.WaitForCompletion();
    }

    public void MoveOriginPos()
    {
        this.transform.position = new Vector3(-7.39f, 2.85f, -5);
    }

    public void MoveTutorialPos()
    {
        this.transform.position = new Vector3(-7.95f, -2.88f, -5);
    }
    public void MoveTutorialSkillPos()
    {
        this.transform.position = new Vector3(-7.59f, -3.87f, -5);
    }
}
