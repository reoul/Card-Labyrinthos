using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//체력바 스크립트
public class HpBar : MonoBehaviour
{
    public GameObject parent;   //이 체력바 부모(플레이어, 적 등등)
    public int hp;              //현재 체력
    public int max_hp;          //최대 체력
    public int sheld;           //방어력
    [SerializeField] GameObject sheldObj;       //실드 오브젝트
    [SerializeField] GameObject hpbar;          //체력바 오브젝트
    [SerializeField] TMP_Text hptext;           //체력 텍스트
    [SerializeField] TMP_Text sheldtext;        //실드 텍스트

    public void Init()          //초기화, 게임 시작할때 실행
    {
        ShowText();
        UpdateHpBar();
    }

    public void SetHP(int hp)
    {
        this.hp = hp;
        this.max_hp = hp;
        Init();
    }

    public void UpdateHp()      //현재 데이터로 텍스트랑 체력바 게이지 조정
    {
        ShowText();
        UpdateHpBar();
    }

    void ShowText()         //체력 텍스트 업데이트
    {
        hptext.text = string.Format("{0}/{1}", hp, max_hp);
    }

    void UpdateHpBar()      //체력바 게이지 조정
    {
        float percent = (float)hp / (float)max_hp;
        hpbar.transform.localScale = new Vector3(percent, 1, 1);
    }

    public void Damage(int damage)      //데미지를 주고 싶을 때 매개변수로 해당 수를 넣어주면 체력이 깍인다
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
        if (hp <= 0)         //체력이 0 이하가 되면 죽음
        {
            Dead();
        }
        else
        {
            ShowText();
            UpdateHpBar();
        }
    }

    public void Sheld(int _sheld)       //방어력을 주고 싶을 때 매개변수로 해당 수를 넣어주면 방어력이 증가함
    {
        sheld += _sheld;
        sheldObj.SetActive(true);
        ShowSheldText();
    }

    void ShowSheldText()                //방어력 텍스트 업데이트
    {
        sheldtext.text = sheld.ToString();
    }

    void Dead()         //죽었을때 발동
    {
        hp = 0;
        if (parent.CompareTag("Player"))
            parent.GetComponent<Player>().Dead();
        else
            parent.GetComponent<Enemy>().Dead();
    }
}
