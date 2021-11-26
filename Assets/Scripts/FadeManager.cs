using System;
using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Inst;
    SpriteRenderer SR;

    public static event EventHandler FadeEvent;

    public float fade_speed;

    public bool isActiveFade;

    private WaitForSeconds delay01 = new WaitForSeconds(0.1f);

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

        this.SR = this.GetComponent<SpriteRenderer>();
        this.isActiveFade = false;
    }

    public void Init()
    {
        this.transform.position = Vector3.zero;
    }

    public void FadeIn()
    {
        this.StartCoroutine(this.Fade(false));
    }

    public void FadeOut()
    {
        this.StartCoroutine(this.Fade(true));
    }

    public IEnumerator FadeInOut(IEnumerator fadeOutPrev1 = null, IEnumerator fadeOutPrev2 = null,
        IEnumerator fadeOutPrev3 = null,
        IEnumerator fadeOutAfter1 = null, IEnumerator fadeOutAfter2 = null, IEnumerator fadeOutAfter3 = null,
        IEnumerator fadeInAfter1 = null, IEnumerator fadeInAfter2 = null, IEnumerator fadeInAfter3 = null)
    {
        this.isActiveFade = true;
        yield return this.StartCoroutine(fadeOutPrev1 ?? this.EmptyCoroutine());
        yield return this.StartCoroutine(fadeOutPrev2 ?? this.EmptyCoroutine());
        yield return this.StartCoroutine(fadeOutPrev3 ?? this.EmptyCoroutine());
        yield return this.StartCoroutine(this.Fade(true)); //페이드아웃 실행
        if (FadeEvent != null)
        {
            FadeEvent(this, EventArgs.Empty); //실행 후 이벤트 실행
            FadeEvent = null;
        }

        yield return this.delay01;
        TopBar.Inst.InitPosition();
        RewardManager.Inst.Init();
        BagManager.Inst.Init();
        SkillManager.Inst.Init();
        this.Init();
        yield return this.delay01;
        TopBar.Inst.UpdateText(TOPBAR_TYPE.SCENENAME);
        yield return this.StartCoroutine(fadeOutAfter1 ?? this.EmptyCoroutine());
        yield return this.StartCoroutine(fadeOutAfter2 ?? this.EmptyCoroutine());
        yield return this.StartCoroutine(fadeOutAfter3 ?? this.EmptyCoroutine());
        yield return this.StartCoroutine(this.Fade(false)); //다시 페이드인 실행
        yield return this.StartCoroutine(fadeInAfter1 ?? this.EmptyCoroutine());
        yield return this.StartCoroutine(fadeInAfter2 ?? this.EmptyCoroutine());
        yield return this.StartCoroutine(fadeInAfter3 ?? this.EmptyCoroutine());
        this.isActiveFade = false;
    }

    IEnumerator EmptyCoroutine()
    {
        yield return null;
    }

    IEnumerator Fade(bool isOut)
    {
        float alpha = isOut ? 0 : 1;
        if (isOut)
            while (alpha < 1)
            {
                alpha += Time.deltaTime * this.fade_speed;
                this.SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
        else
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * this.fade_speed;
                this.SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
    }
}
