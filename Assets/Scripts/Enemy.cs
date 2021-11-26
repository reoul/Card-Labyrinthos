using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HpBar hpbar;         //적 체력바

    public Monster monster;
    public int pattenIndex;          //플레이어에게 줄 데미지
    public int weaknessNum;     //약점카드 숫자
    [SerializeField] PATTERN curPatten;
    int lastPatten;
    int lastWeaknessNum;
    int fixedWeaknessNum = -1;
    public int force;
    public bool isVampire;
    public bool isWeaknessHidden;
    public bool isPattenHidden;
    public float attackDelay;
    public Transform hitPos;        //전투 중 카드가 날아갈 곳

    [SerializeField] SpriteRenderer patten_sprite;
    public TMP_Text pattenIndexTMP;        //데미지 텍스트
    public TMP_Text weaknessTMP;      //약점카드 테스트
    public GameObject[] ui;

    private void Start()
    {
        //fixedWeaknessNum = -1;
        this.pattenIndex = 0;
        this.weaknessNum = 0;
        this.lastPatten = 99;
        this.lastWeaknessNum = 99;
        this.force = 0;
        this.isPattenHidden = false;
        this.isWeaknessHidden = false;
        this.RandomPatten();
        this.UpdateStateText();
    }

    public void RandomPatten()      //랜덤 패턴 함수, 턴 끝날때 실행
    {
        do
        {
            this.weaknessNum = Random.Range(2, 6);
        } while (this.weaknessNum == this.lastWeaknessNum);

        this.lastWeaknessNum = this.weaknessNum;
        if (this.fixedWeaknessNum != -1)
        {
            this.weaknessNum = this.fixedWeaknessNum;
            this.lastWeaknessNum = this.weaknessNum;
            this.fixedWeaknessNum = -1;
        }
        int patten;
        do
        {
            patten = Random.Range(0, 4);
        } while (patten == this.lastPatten);

        this.lastPatten = patten;
        switch (patten)
        {
            case 0:
                this.curPatten = this.monster.pattern_1;
                break;
            case 1:
                this.curPatten = this.monster.pattern_2;
                break;
            case 2:
                this.curPatten = this.monster.pattern_3;
                break;
            case 3:
                this.curPatten = this.monster.pattern_4;
                break;
        }

        this.pattenIndex = this.curPatten.index;
    }


    public void UseTurn()           //적 턴 시작할때 호출됨
    {
        switch (this.curPatten.pattern_type)
        {
            case PATTERN_TYPE.ATTACK:
                EffectManager.Inst.CreateEffectObj(EffectObjType.HIT, Player.Inst.transform.position + new Vector3(0, 1, -15), 0.15f + this.attackDelay);
                this.GetComponent<Animator>().SetTrigger("Attack");         //공격 애니메이션 실행
                break;
            case PATTERN_TYPE.HEAL:
                this.Heal();
                break;
        }
    }

    public void Heal()
    {
        EffectManager.Inst.CreateEffectObj(EffectObjType.HEAL, this.hitPos.position + new Vector3(0, 0, -15), 0, 0.7f);
        this.hpbar.Heal(this.pattenIndex);
    }

    public void Sheld(int _sheld)
    {
        EffectManager.Inst.CreateEffectObj(EffectObjType.SHELD, this.hitPos.position + new Vector3(0, 0, -15), 0, 0.7f);
        this.hpbar.Sheld(_sheld);
    }

    public void Attack()
    {
        if (this.isVampire)
        {
            int damage = Player.Inst.hpbar.sheld - this.pattenIndex + this.force;
            this.hpbar.Heal(damage < 0 ? Mathf.Abs(damage) : 0);
        }
        SoundManager.Inst.Play(BATTLESOUND.HIT);
        Player.Inst.Damage(this.pattenIndex + this.force);
        this.pattenIndex = 0;
    }

    public void UpdateStateText()           //텍스트 최신화
    {
        switch (this.curPatten.pattern_type)
        {
            case PATTERN_TYPE.ATTACK:
                this.patten_sprite.sprite = this.isPattenHidden ? null : StageManager.Inst.attackSprite;
                this.pattenIndexTMP.text = this.isPattenHidden ? "???" : (this.pattenIndex + this.force).ToString();
                this.pattenIndexTMP.color = Color.red;
                break;
            case PATTERN_TYPE.HEAL:
                this.patten_sprite.sprite = this.isPattenHidden ? null : StageManager.Inst.healSprite;
                this.pattenIndexTMP.text = this.isPattenHidden ? "???" : this.pattenIndex.ToString();
                this.pattenIndexTMP.color = new Color(60f / 255, 180f / 255, 60f / 255);
                break;
        }

        this.weaknessTMP.text = this.isWeaknessHidden ? "?" : (this.weaknessNum + 1).ToString();
        this.isPattenHidden = false;
        this.isWeaknessHidden = false;
    }

    public void Damage(int damage)          //적이 공격 당할때 호출
    {
        SoundManager.Inst.Play(BATTLESOUND.HIT);
        this.hpbar.Damage(damage);
        DebuffManager.Inst.turnDamage += damage;
    }

    public void Dead()              //적이 죽을때 호출
    {
        for (int i = 0; i < this.ui.Length; i++) this.ui[i].SetActive(false);
        this.hpbar.UpdateHp();
        this.pattenIndex = 0;
        this.GetComponent<Animator>().SetTrigger("Dead");
    }

    public void DeadAnimationFinish()       //Dead 애니메이션이 다 끝날때 호출 
    {
        EnemyManager.Inst.enemys.Remove(this);
        TurnManager.Inst.isFinish = true;
        if (EnemyManager.Inst.enemys.Count == 0)
        {
            if (this.monster.type != MONSTER_TYPE.BOSS)
            {
                SoundManager.Inst.Play(BATTLESOUND.GAME_WIN);
                this.StartCoroutine(TurnManager.Inst.ShowReward());
            }
            else
            {
                GameManager.Inst.Ending();
            }
            //CardManager.Inst.FinishBattle();
        }
        Destroy(this.hpbar.gameObject);
        Destroy(this.gameObject);
    }
    public void SetFixedWeaknessNum(int index)
    {
        this.fixedWeaknessNum = index;
    }
}
