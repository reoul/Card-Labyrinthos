using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagManager : Singleton<BagManager>
{
    private readonly int[] cardMax = { 18, 15, 12, 9, 6, 3 };

    public bool isOpen;

    private void Awake()
    {
        ExistInstance(this);
    }

    [SerializeField] private List<TMP_Text> card_text;
    [SerializeField] private List<SpriteRenderer> skill_spriteRenderer;
    [SerializeField] private List<GameObject> unlockObjs;

    public void Init()
    {
        transform.position = new Vector3(0, 0, -4);
    }

    public void Open()
    {
        if (isOpen)
        {
            Close();
            return;
        }

        if (MapManager.Inst.tutorialIndex == 2)
        {
            MapManager.Inst.isTutorialOpenBag = true;
            TalkWindow.Inst.SetFlagNext(true);
            TalkWindow.Inst.SetSkip(true);
            TalkWindow.Inst.index2 = 1;
            MapManager.Inst.tutorialIndex++;
        }

        GameManager.Inst.CloseAllUI();
        isOpen = true;
        UpdateText();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void UpdateText()
    {
        for (var i = 0; i < card_text.Count; i++)
        {
            card_text[i].text = string.Format($"{CardManager.Inst.cardDeck[i].ToString()}/{cardMax[i].ToString()}");
        }

        for (var i = 0; i < skill_spriteRenderer.Count; i++)
        {
            skill_spriteRenderer[i].color = new Color(0, 0, 0, 0.5f);
            skill_spriteRenderer[i].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(0, 0, 0, 0.5f);
            if (CardManager.Inst.cardDeck[0] >= 1)
            {
                skill_spriteRenderer[i].color = Color.white;
                skill_spriteRenderer[i].transform.GetChild(0).GetComponent<TMP_Text>().color = Color.black;
                unlockObjs[i].SetActive(true);
            }
        }
    }

    public void Close()
    {
        isOpen = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
