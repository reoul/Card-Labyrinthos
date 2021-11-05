using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkWindow : MonoBehaviour
{
    [SerializeField] Ghost ghost;
    [SerializeField] TMP_Text talkTMP;
    List<string> talk;
    bool isSkip = false;
    bool next = false;
    public int index = 0;

    private void Awake()
    {
        talk = new List<string>();
        talk.Add("드디어 일어났군. 일단 혼란스럽겠지만 내 얘길 들어보게");
        talk.Add("1231 2312312 33212321 123123123 1231231223 123");
        talk.Add("alksdf jisdfjlf djlajslkdajdf iwjlf");
    }
    public void ShowTalk()
    {
        StartCoroutine(TalkCorutine());
        //ghost.HideTalk();
    }

    IEnumerator TalkCorutine()
    {
        for (int i = 0; i < talk.Count; i++)
        {
            yield return StartCoroutine(ShowTalkCorutine(i));
        }
        ghost.HideTalk();
    }

    IEnumerator ShowTalkCorutine(int index)
    {
        for (int i = 0; i < talk[index].Length; i++)
        {
            if (isSkip)
            {
                //i = talk[index].Length - 1;
                isSkip = false;
                break;
            }
            talkTMP.text = talk[index].Substring(0, i + 1);
            if (talk[index][i] == ' ')
                yield return new WaitForSeconds(0.2f);
            yield return new WaitForSeconds(0.1f);
        }
        while (true)
        {
            if (next)
            {
                next = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        this.index++;
        yield return null;
    }

    private void OnMouseUp()
    {
        if (talkTMP.text.Length == talk[index].Length)
        {
            next = true;
        }
        else
        {
            talkTMP.text = talk[index];
            isSkip = true;
        }
    }
}
