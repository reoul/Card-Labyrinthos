using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialBook : MonoBehaviour
{
    public TMP_Text[] texts;
    public string[] contents;
    public GameObject[] icons;

    void Awake()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            contents[i] = texts[i].text;
            texts[i].text = "";
        }
    }

    public IEnumerator LoadTextTyping(int index)
    {
        for (int i = 0; i < contents[index].Length; i++)
        {
            texts[index].text = contents[index].Substring(0, i + 1);
            //texts[index].text = texts[index].text.Replace("\\n", "\n");
            yield return new WaitForSeconds(0.03f);
        }
    }
}
