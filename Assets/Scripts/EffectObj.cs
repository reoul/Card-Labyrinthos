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
            case EffectObjType.Sheld:
                SheldAnimation();
                break;
            case EffectObjType.Heal:
                HealAnimation();
                break;
        }
    }

    private void SheldAnimation()
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
