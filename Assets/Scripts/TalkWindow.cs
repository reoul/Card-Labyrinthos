using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkWindow : MonoBehaviour
{
    [SerializeField] Ghost ghost;
    public void ShowTalk()
    {
        Debug.Log("설명 텍스트");
        ghost.HideTalk();
    }
}
