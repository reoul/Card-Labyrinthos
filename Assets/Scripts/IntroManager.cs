using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    bool onStartButton = false;   //마우스가 필드 위에 있는지

    private void Start()
    {
        SoundManager.Inst.Play(BACKGROUNDSOUND.INTRO);
        StartCoroutine(CardManager.Inst.StartIntroCard());
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
