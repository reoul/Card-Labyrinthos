using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Tomb : MouseInteractionObject
{
    private bool isGet;
    private bool isFade = true;

    private void OnMouseUp()
    {
        if (!OnMouse || isGet || isFade)
        {
            return;
        }

        StartCoroutine(TutorialManager.Inst.GetCardCoroutine());
        isFade = true;
    }

    public IEnumerator SetLook()
    {
        Tween fade = GetComponent<SpriteRenderer>().DOFade(1, 1);
        yield return fade.WaitForCompletion();

        isFade = false;
    }
}
