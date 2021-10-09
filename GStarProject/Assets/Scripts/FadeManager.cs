using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Inst { get; private set; }
    SpriteRenderer SR;

    public static event EventHandler FadeEvent;

    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
        SR = this.GetComponent<SpriteRenderer>();
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(false));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(true));
    }

    public IEnumerator FadeInOut(IEnumerator enumerator = null)
    {
        yield return StartCoroutine(Fade(true));       //페이드 실행
        if (FadeEvent != null)
            FadeEvent(this, EventArgs.Empty);           //실행 후 이벤트 실행
        yield return StartCoroutine(Fade(false));                   //다시 페이드 실행
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    IEnumerator Fade(bool isOut)
    {
        float alpha = isOut ? 0 : 1;
        if (isOut)
            while (alpha < 1)
            {
                alpha += Time.deltaTime * 2;
                SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
        else
            while (alpha > 0)
            {
                alpha -= Time.deltaTime / 2;
                SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
    }
}
