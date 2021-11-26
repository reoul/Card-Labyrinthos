using TMPro;
using UnityEngine;

//체력바 스크립트
public class HpBar : MonoBehaviour
{
    public GameObject parent;   //이 체력바 부모(플레이어, 적 등등)
    public int hp;              //현재 체력
    public int max_hp;          //최대 체력
    public int sheld;           //방어력
    public int turnStartSheld;
    [SerializeField] GameObject sheldObj;       //실드 오브젝트
    [SerializeField] GameObject hpbar;          //체력바 오브젝트
    [SerializeField] TMP_Text hptext;           //체력 텍스트
    public TMP_Text sheldtext;        //실드 텍스트

    public void Init()          //초기화, 게임 시작할때 실행
    {
        this.UpdateHp();
        this.turnStartSheld = 0;
    }

    public void SetHP(int hp)
    {
        this.hp = hp;
        this.max_hp = hp;
        this.parent = this.transform.parent.gameObject;
        this.UpdateHp();
    }

    public void SetTurnStartSheld(int sheld)
    {
        this.turnStartSheld = sheld;
    }

    public void UpdateHp()      //현재 데이터로 텍스트랑 체력바 게이지 조정
    {
        this.ShowText();
        this.UpdateHpBar();
    }

    void ShowText()         //체력 텍스트 업데이트
    {
        this.hptext.text = string.Format("{0}/{1}", this.hp, this.max_hp);
    }

    void UpdateHpBar()      //체력바 게이지 조정
    {
        float percent = this.hp / (float) this.max_hp;
        this.hpbar.transform.localScale = new Vector3(percent, 1, 1);
    }

    public void Damage(int damage)      //데미지를 주고 싶을 때 매개변수로 해당 수를 넣어주면 체력이 깍인다
    {
        if (this.sheld > 0)
        {
            this.sheld -= damage;
            this.ShowSheldText();
            if (this.sheld <= 0)
            {
                this.hp += this.sheld;
                this.sheld = 0;
                this.sheldObj.SetActive(false);
            }
        }
        else
            this.hp -= damage;
        if (this.parent.tag == "Player")
            PlayerManager.Inst.hp = this.hp;
        if (this.hp <= 0)         //체력이 0 이하가 되면 죽음
        {
            this.Dead();
        }
        else
        {
            this.ShowText();
            this.UpdateHpBar();
        }
    }

    public void Sheld(int _sheld)       //방어력을 주고 싶을 때 매개변수로 해당 수를 넣어주면 방어력이 증가함
    {
        SoundManager.Inst.Play(BATTLESOUND.SHELD);
        this.sheld += _sheld;
        this.sheldObj.SetActive(true);
        this.ShowSheldText();
    }

    public void Heal(int index)
    {
        SoundManager.Inst.Play(BATTLESOUND.HEAL);
        this.hp = Mathf.Clamp(this.hp + index, 0, this.max_hp);
        this.UpdateHp();
    }

    void ShowSheldText()                //방어력 텍스트 업데이트
    {
        this.sheldtext.text = this.sheld.ToString();
    }

    void Dead()         //죽었을때 발동
    {
        this.hp = 0;
        if (this.parent.tag == "Player")
            this.parent.GetComponent<Player>().Dead();
        else
            this.parent.GetComponent<Enemy>().Dead();
    }
}
