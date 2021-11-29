using System;
using System.Collections;
using UnityEngine;

public class FadeManager : Singleton<FadeManager>
{
    private SpriteRenderer SR;

    public static event EventHandler FadeEvent;

    public float fade_speed;

    public bool isActiveFade;

    private readonly WaitForSeconds delay01 = new WaitForSeconds(0.1f);

    private void Awake()
    {
        ExistInstance(this);
        SR = GetComponent<SpriteRenderer>();
        isActiveFade = false;
    }

    private void Init()
    {
        transform.position = Vector3.zero;
    }

    public IEnumerator FadeInOut(IEnumerator fadeOutPrev1 = null, IEnumerator fadeOutPrev2 = null,
        IEnumerator fadeOutPrev3 = null,
        IEnumerator fadeOutAfter1 = null, IEnumerator fadeOutAfter2 = null, IEnumerator fadeOutAfter3 = null,
        IEnumerator fadeInAfter1 = null, IEnumerator fadeInAfter2 = null, IEnumerator fadeInAfter3 = null)
    {
        isActiveFade = true;
        yield return StartCoroutine(fadeOutPrev1 ?? EmptyCoroutine());
        yield return StartCoroutine(fadeOutPrev2 ?? EmptyCoroutine());
        yield return StartCoroutine(fadeOutPrev3 ?? EmptyCoroutine());
        
        yield return StartCoroutine(Fade(true)); //페이드아웃 실행
        
        if (FadeEvent != null)
        {
            FadeEvent(this, EventArgs.Empty); //실행 후 이벤트 실행
            FadeEvent = null;
        }

        yield return delay01;
        TopBar.Inst.InitPosition();
        RewardManager.Inst.Init();
        BagManager.Inst.Init();
        SkillManager.Inst.Init();
        Init();
        
        yield return delay01;
        TopBar.Inst.UpdateText(TOPBAR_TYPE.SCENENAME);
        yield return StartCoroutine(fadeOutAfter1 ?? EmptyCoroutine());
        yield return StartCoroutine(fadeOutAfter2 ?? EmptyCoroutine());
        yield return StartCoroutine(fadeOutAfter3 ?? EmptyCoroutine());
        
        yield return StartCoroutine(Fade(false)); //다시 페이드인 실행
        
        yield return StartCoroutine(fadeInAfter1 ?? EmptyCoroutine());
        yield return StartCoroutine(fadeInAfter2 ?? EmptyCoroutine());
        yield return StartCoroutine(fadeInAfter3 ?? EmptyCoroutine());
        isActiveFade = false;
    }

    // 페이드 함수에서 fade 이벤트가 널일때 대신 호출
    private IEnumerator EmptyCoroutine()
    {
        yield return null;
    }

    private IEnumerator Fade(bool isOut)
    {
        float alpha = isOut ? 0 : 1;
        if (isOut)
        {
            while (alpha < 1)
            {
                alpha += Time.deltaTime * fade_speed;
                SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * fade_speed;
                SR.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
