using DG.Tweening;
using UnityEngine;

public class EffectObj : MonoBehaviour
{
    EffectObjType type;

    public void Init(EffectObjType _type)
    {
        this.type = _type;
        switch (this.type)
        {
            case EffectObjType.HIT:
                break;
            case EffectObjType.SHELD:
                this.SheldAnimation();
                break;
            case EffectObjType.HEAL:
                this.HealAnimation();
                break;
        }
    }

    void SheldAnimation()
    {
        this.transform.DOMoveY(this.transform.position.y - 0.5f, 1);
        this.transform.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
    }

    void HealAnimation()
    {
        this.transform.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
    }
}
