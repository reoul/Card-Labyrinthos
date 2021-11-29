using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public HpBar hpbar; //적 체력바

    public Monster monster;
    public int pattenIndex; //플레이어에게 줄 데미지
    public int weaknessNum; //약점카드 숫자
    [SerializeField] private PATTERN curPatten;
    private int lastPatten;
    private int lastWeaknessNum;
    private int fixedWeaknessNum = -1; // 약점 숫자를 고정해야 할 때 사용
    public int force;
    public bool isVampire;
    public bool isWeaknessHidden;
    public bool isPattenHidden;
    public float attackDelay;
    public Transform hitPos; //전투 중 카드가 날아갈 곳

    [SerializeField] private SpriteRenderer patten_sprite;
    public TMP_Text pattenIndexTMP; //데미지 텍스트
    public TMP_Text weaknessTMP; //약점카드 테스트

    private void Start()
    {
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

    public void RandomPatten() //랜덤 패턴 함수, 턴 끝날때 실행
    {
        do
        {
            weaknessNum = Random.Range(2, 6);
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


    public void UseTurn() //적 턴 시작할때 호출됨
    {
        switch (curPatten.pattern_type)
        {
            case PATTERN_TYPE.ATTACK:
                EffectManager.Inst.CreateEffectObj(EffectObjType.Hit,
                    Player.Inst.transform.position + new Vector3(0, 1, -15), 0.15f + attackDelay);
                GetComponent<Animator>().SetTrigger("Attack"); //공격 애니메이션 실행
                break;
            case PATTERN_TYPE.HEAL:
                Heal();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Heal()
    {
        EffectManager.Inst.CreateEffectObj(EffectObjType.Heal, hitPos.position + new Vector3(0, 0, -15), 0, 0.7f);
        hpbar.Heal(pattenIndex);
    }

    public void Sheld(int sheld)
    {
        EffectManager.Inst.CreateEffectObj(EffectObjType.Sheld, hitPos.position + new Vector3(0, 0, -15), 0, 0.7f);
        hpbar.Sheld(sheld);
    }

    //공격 애니메이션 끝날 때 호출
    public void Attack()
    {
        if (isVampire)
        {
            int damage = Player.Inst.hpbar.sheld - pattenIndex + force;
            hpbar.Heal(damage < 0 ? Mathf.Abs(damage) : 0);
        }

        SoundManager.Inst.Play(BATTLESOUND.Hit);
        Player.Inst.Damage(pattenIndex + force);
        pattenIndex = 0;
    }

    public void UpdateStateText() //텍스트 최신화
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
            default:
                throw new ArgumentOutOfRangeException();
        }

        weaknessTMP.text = isWeaknessHidden ? "?" : (weaknessNum + 1).ToString();
        isPattenHidden = false;
        isWeaknessHidden = false;
    }

    public void Damage(int damage) //적이 공격 당할때 호출
    {
        SoundManager.Inst.Play(BATTLESOUND.Hit);
        hpbar.Damage(damage);
        DebuffManager.Inst.turnDamage += damage;
    }

    public void Dead() //적이 죽을때 호출
    {
        hpbar.UpdateHp();
        pattenIndex = 0;
        GetComponent<Animator>().SetTrigger("Dead");
    }

    public void DeadAnimationFinish() //Dead 애니메이션이 다 끝날때 호출 
    {
        EnemyManager.Inst.enemys.Remove(this);
        TurnManager.Inst.isFinish = true;
        if (EnemyManager.Inst.enemys.Count == 0)
        {
            if (monster.type != MONSTER_TYPE.BOSS)
            {
                SoundManager.Inst.Play(BATTLESOUND.GameWin);
                StartCoroutine(TurnManager.Inst.ShowReward());
            }
            else
            {
                GameManager.Inst.Ending();
            }
        }

        Destroy(hpbar.gameObject);
        Destroy(gameObject);
    }

    public void SetFixedWeaknessNum(int index)
    {
        fixedWeaknessNum = index;
    }
}
