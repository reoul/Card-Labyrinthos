using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Inst = null;
    SpriteRenderer SR;

    public static event EventHandler FadeEvent;

    public float fade_speed;

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

    public IEnumerator FadeInOut(IEnumerator FadeOutAfter1 = null, IEnumerator FadeOutAfter2 = null, IEnumerator FadeOutAfter3 = null,
            IEnumerator FadeInAfter1 = null, IEnumerator FadeInAfter2 = null, IEnumerator FadeInAfter3 = null)
    {
        yield return StartCoroutine(Fade(true));       //페이드아웃 실행
        if (FadeEvent != null)
        {
            FadeEvent(this, EventArgs.Empty);           //실행 후 이벤트 실행
            FadeEvent = null;
        }
        yield return new WaitForSeconds(0.1f);
        TopBar.Inst.UpdateText(TOPBAR_TYPE.SCENENAME);
        if (FadeOutAfter1 != null)
            yield return StartCoroutine(FadeOutAfter1);
        if (FadeOutAfter2 != null)
            yield return StartCoroutine(FadeOutAfter2);
        if (FadeOutAfter3 != null)
            yield return StartCoroutine(FadeOutAfter3);
        yield return StartCoroutine(Fade(false));                   //다시 페이드인 실행
        if (FadeInAfter1 != null)
            yield return StartCoroutine(FadeInAfter1);
        if (FadeInAfter2 != null)
            yield return StartCoroutine(FadeInAfter2);
        if (FadeInAfter3 != null)
            yield return StartCoroutine(FadeInAfter3);
    }

    IEnumerator Fade(bool isOut)
    {
        float alpha = isOut ? 0 : 1;
        if (isOut)
            while (alpha < 1)
            {
                alpha += Time.deltaTime * fade_speed;
                SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
        else
            while (alpha > 0)
            {
                alpha -= Time.deltaTime / fade_speed;
                SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
    }
}
