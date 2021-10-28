using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] TMP_Text[] condition_TMP;

    public void MouseUp(EventData eventData)
    {
        EventManager.Inst.Choice(eventData);
    }

    public void Init()      //이벤트 씬 처음 들었을때 호출, 각 선택지마다 다른 조건을 줌
    {
        int[] rands = new int[3];
        rands[0] = Random.Range(0, 3); //각 이벤트 선택지마다 카드 수 합 랜덤으로 매김
        do
        {
            rands[1] = Random.Range(0, 3);
        } while (rands[0] == rands[1]);
        rands[2] = 3 - rands[0] - rands[1];
        string[,] achieve = { { "[카드 수 합 : 1 ~ 5]", "[카드 수 합 : 6 ~ 10]", "[카드 수 합 : 11 ~ 18]" }, { "[카드 수 합 : 1 ~ 7]", "[카드 수 합 : 8 ~ 14]", "[카드 수 합 : 15 ~ 24]" }, { "[카드 수 합 : 1 ~ 9]", "[카드 수 합 : 10 ~ 18]", "[카드 수 합 : 19 ~ 30]" }, { "[카드 수 합 : 1 ~ 10]", "[카드 수 합 : 11 ~ 20]", "[카드 수 합 : 21 ~ 36]" } };
        int[,] limits = { { 1, 5, 6, 10, 11, 18 }, { 1, 7, 8, 14, 15, 24 }, { 1, 9, 10, 18, 19, 30 }, { 1, 10, 11, 20, 21, 36 } };
        for (int i = 0; i < condition_TMP.Length; i++)
        {
            condition_TMP[rands[i]].text = achieve[TurnManager.Inst.startCardCount - 3, i];
            condition_TMP[rands[i]].transform.parent.GetComponent<EventButton>().limitNumMin = limits[TurnManager.Inst.startCardCount - 3, i * 2];
            condition_TMP[rands[i]].transform.parent.GetComponent<EventButton>().limitNumMax = limits[TurnManager.Inst.startCardCount - 3, i * 2 + 1];
        }
    }
}
