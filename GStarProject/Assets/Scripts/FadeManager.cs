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

    IEnumerator Fade(bool isOut)
    {
        float alpha = isOut ? 0 : 1;
        if (isOut)
            while (alpha < 1)
            {
                alpha += Time.deltaTime;
                SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
        else
            while (alpha > 0)
            {
                alpha -= Time.deltaTime;
                Debug.Log(alpha);
                SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }

        if(FadeEvent != null)
            FadeEvent.Invoke(this, EventArgs.Empty);
    }

    public void TestEvent(object sender, EventArgs e)
    {

    }
}
