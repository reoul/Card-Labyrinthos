using System;
using DG.Tweening;
using UnityEngine;

public class EffectObj : MonoBehaviour
{
    private EffectObjType type;

    public void Init(EffectObjType type)
    {
        this.type = type;
        switch (this.type)
        {
            case EffectObjType.Hit:
                break;
            case EffectObjType.Shield:
                ShieldAnimation();
                break;
            case EffectObjType.Heal:
                HealAnimation();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ShieldAnimation()
    {
        transform.DOMoveY(transform.position.y - 0.5f, 1);
        AnimationObjFade();
    }

    private void HealAnimation()
    {
        AnimationObjFade();
    }

    private void AnimationObjFade()
    {
        transform.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
    }
}
