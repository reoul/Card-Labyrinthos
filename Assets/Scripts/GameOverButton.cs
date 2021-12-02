using System;
using UnityEngine;

public class GameOverButton : MouseInteractionObject
{
    public enum Type
    {
        Title,
        GameEnd
    }

    public Type type;

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
    }

    private void OnMouseUp()
    {
        if (!OnMouse)
        {
            return;
        }

        switch (type)
        {
            case Type.Title:
                ResetManager.Inst.ResetGame();
                break;
            case Type.GameEnd:
                Application.Quit();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
