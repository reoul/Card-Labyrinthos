using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public static BagManager Inst = null;
    int[] cardMax = new int[6] { 18, 15, 12, 9, 6, 3 };

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
        if (isOpen)
        {
            Close();
            return;
        }
        GameManager.Inst.CloseAllUI();
        isOpen = true;
        UpdateText();
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void UpdateText()
    {
        for (int i = 0; i < card_text.Count; i++)
        {
            card_text[i].text = string.Format($"{CardManager.Inst.cardDeck[i]}/{cardMax[i]}");
        }
        for (int i = 0; i < skill_spriteRenderer.Count; i++)
        {
            skill_spriteRenderer[i].color = new Color(0, 0, 0, 0.5f);
            skill_spriteRenderer[i].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(0, 0, 0, 0.5f);
            if (CardManager.Inst.cardDeck[i] >= i * 2 + 1)
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
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
