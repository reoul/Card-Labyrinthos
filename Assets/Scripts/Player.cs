using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Inst;

    public HpBar hpbar;

    private void Awake()
    {
        Inst = this;
    }

    public void Attack()
    {
        //this.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void Sheld(int _sheld)
    {
        EffectManager.Inst.CreateEffectObj(EffectObjType.Sheld, transform.position + new Vector3(0, 2, -15), 0, 0.7f);
        hpbar.Sheld(_sheld);
    }

    public void Damage(int damage)          //플레이어가 공격 당할때 호출
    {
        //EffectManager.Inst.CreateEffectObj(EffectObjType.HIT, this.transform.position + new Vector3(0, 1, -15));
        hpbar.Damage(damage);
    }

    public void Dead()
    {
        SoundManager.Inst.Play(BATTLESOUND.GAME_FAILD);
        GetComponent<Animator>().SetTrigger("Dead");
        hpbar.UpdateHp();
        TurnManager.Inst.isFinish = true;
    }

    public void DeadAnimationFinish()
    {
        GameManager.Inst.GameOver();
        Destroy(hpbar.gameObject);
        Destroy(gameObject);
    }
}
