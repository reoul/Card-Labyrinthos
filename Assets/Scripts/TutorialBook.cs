using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialBook : MonoBehaviour
{
    public TMP_Text[] texts;
    public string[] contents;
    public GameObject[] icons;

    void Awake()
    {
        for (int i = 0; i < this.texts.Length; i++)
        {
            this.contents[i] = this.texts[i].text;
            this.texts[i].text = "";
        }
    }

    public IEnumerator ShowBook()
    {
        while (true)
        {
            this.GetComponent<SpriteRenderer>().color += Color.black * Time.deltaTime;
            if (this.GetComponent<SpriteRenderer>().color.a >= 1)
                break;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator LoadTextTyping(int index)
    {
        for (int i = 0; i < this.contents[index].Length; i++)
        {
            this.texts[index].text = this.contents[index].Substring(0, i + 1);
            //texts[index].text = texts[index].text.Replace("\\n", "\n");
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForEndOfFrame();
        }
    }
}
