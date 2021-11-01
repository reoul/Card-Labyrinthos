using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBookPage : MonoBehaviour
{
    public List<SkillBookCard> choiceCards;
    public List<SkillBookCard> applyCards;

    [SerializeField] List<TMP_Text> TextTMP;
    [SerializeField] List<SpriteRenderer> renderers;

    public void Show()
    {
        SkillManager.Inst.choiceCards = this.choiceCards;
        SkillManager.Inst.applyCards = this.applyCards;
    }
}
