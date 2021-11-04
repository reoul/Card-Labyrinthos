using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AAA
{
    public static Color FadeOut(this Color color)
    {

        return color;
    }
}


public class ColorManager : MonoBehaviour
{
    public static ColorManager Inst = null;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ColorFadeIn()
    {

    }
    public void ColorFadeOut(ref Color color)
    {
        StartCoroutine(ColorFadeOutCorutine(ref color));
    }

    IEnumerator ColorFadeInCorutine(ref Color color)
    {

    }

    IEnumerator ColorFadeOutCorutine(ref Color color)
    {
        while (true)
        {
            color -= Color.black;
        }
        yield return null;
    }
}
