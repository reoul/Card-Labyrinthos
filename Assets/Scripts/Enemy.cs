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
        pattenIndex = 0;
        weaknessNum = 0;
        lastPatten = 99;
        lastWeaknessNum = 99;
        force = 0;
        isPattenHidden = false;
        isWeaknessHidden = false;
        RandomPatten();
        UpdateStateText();
    }

    public void RandomPatten()      //랜덤 패턴 함수, 턴 끝날때 실행
    {
        do
        {
            weaknessNum = Random.Range(1, 6);
        } while (weaknessNum == lastWeaknessNum);
        lastWeaknessNum = weaknessNum;
        if (fixedWeaknessNum != -1)
        {
            weaknessNum = fixedWeaknessNum;
            lastWeaknessNum = weaknessNum;
            fixedWeaknessNum = -1;
        }
        int patten;
        do
        {
            patten = Random.Range(0, 4);
        } while (patten == lastPatten);
        lastPatten = patten;
        switch (patten)
        {
            case 0:
                curPatten = monster.pattern_1;
                break;
            case 1:
                curPatten = monster.pattern_2;
                break;
            case 2:
                curPatten = monster.pattern_3;
                break;
            case 3:
                curPatten = monster.pattern_4;
                break;
        }
        pattenIndex = curPatten.index;
    }


    public void UseTurn()           //적 턴 시작할때 호출됨
    {
        switch (curPatten.pattern_type)
        {
            case PATTERN_TYPE.ATTACK:
                EffectManager.Inst.CreateEffectObj(EffectObjType.HIT, Player.Inst.transform.position + new Vector3(0, 1, -15), 0.15f + attackDelay);
                this.GetComponent<Animator>().SetTrigger("Attack");         //공격 애니메이션 실행
                break;
            case PATTERN_TYPE.HEAL:
                Heal();
                break;
        }
    }

    public void Heal()
    {
        EffectManager.Inst.CreateEffectObj(EffectObjType.HEAL, hitPos.position + new Vector3(0, 0, -15), 0, 0.7f);
        hpbar.Heal(pattenIndex);
    }

    public void Sheld(int _sheld)
    {
        EffectManager.Inst.CreateEffectObj(EffectObjType.SHELD, hitPos.position + new Vector3(0, 0, -15), 0, 0.7f);
        hpbar.Sheld(_sheld);
    }

    public void Attack()
    {
        if (isVampire)
        {
            int damage = Player.Inst.hpbar.sheld - pattenIndex + force;
            hpbar.Heal(damage < 0 ? Mathf.Abs(damage) : 0);
        }
        SoundManager.Inst.Play(BATTLESOUND.HIT);
        Player.Inst.Damage(pattenIndex + force);
        pattenIndex = 0;
    }

    public void UpdateStateText()           //텍스트 최신화
    {
        switch (curPatten.pattern_type)
        {
            case PATTERN_TYPE.ATTACK:
                patten_sprite.sprite = isPattenHidden ? null : StageManager.Inst.attackSprite;
                pattenIndexTMP.text = isPattenHidden ? "???" : (pattenIndex + force).ToString();
                pattenIndexTMP.color = Color.red;
                break;
            case PATTERN_TYPE.HEAL:
                patten_sprite.sprite = isPattenHidden ? null : StageManager.Inst.healSprite;
                pattenIndexTMP.text = isPattenHidden ? "???" : pattenIndex.ToString();
                pattenIndexTMP.color = new Color(60f / 255, 180f / 255, 60f / 255);
                break;
        }
        weaknessTMP.text = isWeaknessHidden ? "?" : (weaknessNum + 1).ToString();
        isPattenHidden = false;
        isWeaknessHidden = false;
    }

    public void Damage(int damage)          //적이 공격 당할때 호출
    {
        SoundManager.Inst.Play(BATTLESOUND.HIT);
        hpbar.Damage(damage);
        DebuffManager.Inst.turnDamage += damage;
    }

    public void Dead()              //적이 죽을때 호출
    {
        for (int i = 0; i < ui.Length; i++)
            ui[i].SetActive(false);
        hpbar.UpdateHp();
        pattenIndex = 0;
        this.GetComponent<Animator>().SetTrigger("Dead");
    }

    public void DeadAnimationFinish()       //Dead 애니메이션이 다 끝날때 호출 
    {
        EnemyManager.Inst.enemys.Remove(this);
        TurnManager.Inst.isFinish = true;
        if (EnemyManager.Inst.enemys.Count == 0)
        {
            SoundManager.Inst.Play(BATTLESOUND.GAME_WIN);
            StartCoroutine(TurnManager.Inst.ShowReward());
            //CardManager.Inst.FinishBattle();
        }
        Destroy(hpbar.gameObject);
        Destroy(this.gameObject);
    }
    public void SetFixedWeaknessNum(int index)
    {
        fixedWeaknessNum = index;
    }
}
