using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpBar : MonoBehaviour
{
    public GameObject parent;
    public int hp;
    public int max_hp;
    public int sheld;
    [SerializeField] GameObject sheldObj;
    [SerializeField] GameObject hpbar;
    [SerializeField] TMP_Text hptext;
    [SerializeField] TMP_Text sheldtext;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        ShowText();
        UpdateHpBar();
    }

    public void UpdateHp()
    {
        ShowText();
        UpdateHpBar();
    }

    void ShowText()
    {
        hptext.text = string.Format("{0}/{1}", hp, max_hp);
    }

    void UpdateHpBar()
    {
        float percent = (float)hp / (float)max_hp;
        hpbar.transform.localScale = new Vector3(percent, 1, 1);
    }

    public void Damage(int damage)
    {
        if (sheld > 0)
        {
            sheld -= damage;
            ShowSheldText();
            if (sheld <= 0)
            {
                hp += sheld;
                sheld = 0;
                sheldObj.SetActive(false);
            }
        }
        else
            hp -= damage;
        if(hp <= 0)
        {
            Dead();
        }
        else
        {
            ShowText();
            UpdateHpBar();
        }
    }

    public void Sheld(int _sheld)
    {
        sheld += _sheld;
        sheldObj.SetActive(true);
        ShowSheldText();
    }

    void ShowSheldText()
    {
        sheldtext.text = sheld.ToString();
    }

    void Dead()
    {
        hp = 0;
        if(parent.CompareTag("Player"))
            parent.GetComponent<Player>().Dead();
        else
            parent.GetComponent<Enemy>().Dead();
    }
}
