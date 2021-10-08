using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void IconMouseDown()
    {
        Debug.Log("donw");
    }
    public void IconMouseUp()
    {
        FadeManager.Inst.FadeOut();
    }
}
