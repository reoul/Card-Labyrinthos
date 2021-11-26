using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public static BagManager Inst;
    readonly int[] cardMax = new int[6] {18, 15, 12, 9, 6, 3};

    public bool isOpen;

    void Awake()
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

    [SerializeField] List<TMP_Text> card_text;
    [SerializeField] List<SpriteRenderer> skill_spriteRenderer;
    [SerializeField] List<GameObject> unlockObjs;

    public void Init()
    {
        this.transform.position = new Vector3(0, 0, -4);
    }

    public void Open()
    {
        if (this.isOpen)
        {
            this.Close();
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
        this.isOpen = true;
        this.UpdateText();
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void UpdateText()
    {
        for (int i = 0; i < this.card_text.Count; i++)
        {
            this.card_text[i].text = string.Format($"{CardManager.Inst.cardDeck[i].ToString()}/{this.cardMax[i].ToString()}");
        }

        for (int i = 0; i < this.skill_spriteRenderer.Count; i++)
        {
            this.skill_spriteRenderer[i].color = new Color(0, 0, 0, 0.5f);
            this.skill_spriteRenderer[i].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(0, 0, 0, 0.5f);
            if (CardManager.Inst.cardDeck[0] >= 1)
            {
                this.skill_spriteRenderer[i].color = Color.white;
                this.skill_spriteRenderer[i].transform.GetChild(0).GetComponent<TMP_Text>().color = Color.black;
                this.unlockObjs[i].SetActive(true);
            }
        }
    }

    public void Close()
    {
        this.isOpen = false;
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
