using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBookCardButton : MonoBehaviour
{
    public SkillBookCard parent;

    [SerializeField] bool onButton = false;

    enum TYPE { UP, DOWN, APPLY }

    [SerializeField] TYPE type;

    private void OnMouseUp()
    {
        if (onButton)
            switch (type)
            {
                case TYPE.UP:
                    parent.Up();
                    break;
                case TYPE.DOWN:
                    parent.Down();
                    break;
                case TYPE.APPLY:
                    SkillManager.Inst.ApplyCardAll();
                    break;
            }
    }

    private void OnMouseEnter()
    {
        onButton = true;
    }

    private void OnMouseExit()
    {
        onButton = false;
    }
}
