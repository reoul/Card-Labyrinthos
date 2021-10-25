using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    public static TopBar Inst = null;
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

    private void Start()
    {
        UpdateText();
    }

    public TMP_Text hpTMP;
    public TMP_Text cardPieceTMP;

    public void UpdateText()
    {
        hpTMP.text = PlayerManager.Inst.hpString;
        cardPieceTMP.text = PlayerManager.Inst.card_piece.ToString();
    }
}
