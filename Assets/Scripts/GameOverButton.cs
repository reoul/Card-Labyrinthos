using System;
using UnityEngine;

public class GameOverButton : MonoBehaviour
{
    public enum Type
    {
        Title,
        GameEnd
    }

    public Type type;

    private void OnMouseEnter()
    {
        SoundManager.Inst.Play(EVENTSOUND.ChoiceMouseup);
    }

    private void OnMouseUp()
    {
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
