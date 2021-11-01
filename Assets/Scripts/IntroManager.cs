using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    bool onStartButton = false;   //마우스가 필드 위에 있는지

    private void Start()
    {
        SoundManager.Inst.BackGroundPlay(BACKGROUNDSOUND.INTRO);
    }

    void OnMouseEnter()
    {
        onStartButton = true;
    }
    void OnMouseExit()
    {
        onStartButton = false;
    }

    private void OnMouseUp()
    {
        MapManager.Inst.LoadTutorialScene();
    }
}
