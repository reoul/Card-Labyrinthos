using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObj : MonoBehaviour
{
    EffectObjType type;

    public void Init(EffectObjType _type)
    {
        type = _type;
        switch (type)
        {
            case EffectObjType.HIT:
                break;
            case EffectObjType.SHELD:
                SheldAnimation();
                break;
            case EffectObjType.HEAL:
                HealAnimation();
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
