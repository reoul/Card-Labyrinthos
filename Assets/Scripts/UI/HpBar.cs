using TMPro;
using UnityEngine;

//체력바 스크립트
public class HpBar : MonoBehaviour
{
    public GameObject parent; //이 체력바 부모(플레이어, 적 등등)
    public int Hp { get; set; } //현재 체력
    public int MAXHp { get; set; } //최대 체력
    public int Shield { get; set; } //방어력
    public int TurnStartShield { get; set; }
    [SerializeField] private GameObject sheldObj; //실드 오브젝트
    [SerializeField] private GameObject hpbar; //체력바 오브젝트
    [SerializeField] private TMP_Text hptext; //체력 텍스트
    public TMP_Text sheldtext; //실드 텍스트

    public void Init() //초기화, 게임 시작할때 실행
    {
        UpdateHp();
        TurnStartShield = 0;
    }

    public void SetHp(int hp)
    {
        Hp = hp;
        MAXHp = hp;
        parent = transform.parent.gameObject;
        UpdateHp();
    }

    public void SetTurnStartShield(int shield)
    {
        TurnStartShield = shield;
    }

    public void UpdateHp() //현재 데이터로 텍스트랑 체력바 게이지 조정
    {
        ShowText();
        UpdateHpBar();
    }

    private void ShowText() //체력 텍스트 업데이트
    {
        hptext.text = $"{Hp.ToString()}/{MAXHp.ToString()}";
    }

    private void UpdateHpBar() //체력바 게이지 조정
    {
        float percent = Hp / (float) MAXHp;
        hpbar.transform.localScale = new Vector3(percent, 1, 1);
    }

    public void Damage(int damage) //데미지를 주고 싶을 때 매개변수로 해당 수를 넣어주면 체력이 깍인다
    {
        if (Shield > 0)
        {
            Shield -= damage;
            ShowShieldText();
            if (Shield <= 0)
            {
                Hp += Shield;
                Shield = 0;
                sheldObj.SetActive(false);
            }
        }
        else
        {
            Hp -= damage;
        }

        if (parent.CompareTag("Player"))
        {
            PlayerManager.Inst.Hp = Hp;
        }

        if (Hp <= 0) //체력이 0 이하가 되면 죽음
        {
            Dead();
        }
        else
        {
            ShowText();
            UpdateHpBar();
        }
    }

    public void AddShield(int shield) //방어력을 주고 싶을 때 매개변수로 해당 수를 넣어주면 방어력이 증가함
    {
        SoundManager.Inst.Play(BATTLESOUND.Shield);
        Shield += shield;
        sheldObj.SetActive(true);
        ShowShieldText();
    }

    public void Heal(int index)
    {
        SoundManager.Inst.Play(BATTLESOUND.Heal);
        Hp = Mathf.Clamp(Hp + index, 0, MAXHp);
        UpdateHp();
    }

    private void ShowShieldText() //방어력 텍스트 업데이트
    {
        sheldtext.text = Shield.ToString();
    }

    private void Dead() //죽었을때 발동
    {
        Hp = 0;
        if (parent.CompareTag("Player"))
        {
            parent.GetComponent<Player>().Dead();
        }
        else
        {
            parent.GetComponent<Enemy>().Dead();
        }
    }
}
