using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButton : MonoBehaviour
{
    public enum Type { TITLE, GAME_END }
    public Type type;

    private void OnMouseUp()
    {
        switch (type)
        {
            case Type.TITLE:
                ResetManager.Inst.ResetGame();
                break;
            case Type.GAME_END:
                Application.Quit();
                break;
        }
    }
}
