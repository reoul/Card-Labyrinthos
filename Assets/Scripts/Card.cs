using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int original_Num { get; private set; }
    public int final_Num { get; private set; }
    [SerializeField] TMP_Text num_TMP;

    public PRS originPRS;
    public Transform parent;
    int a = 10;

    public void Setup(int num)
    {
        a = 20;
        this.original_Num = num;
        this.final_Num = num;
        num_TMP.text = (num + 1).ToString();
    }

    private void Awake()
    {
        parent = transform.parent;
    }

    //string SetContent(Item item)
    //{
    //    string content = "";
    //    if (item.attack > 0)
    //    {
    //        if(item.vulnerable > 0)
    //            content = string.Format("피해를 {0} 줍니다.\n취약을 {1} 부여합니다.", item.attack, item.vulnerable);
    //        else if(item.copy_throw)
    //            content = string.Format("피해를 {0} 줍니다.\n이 카드를 1장 복사해 버린 카드 더미에 섞어넣습니다.", item.attack);
    //        else if(item.allememy)
    //            content = string.Format("적 전체에게 피해를 {0} 줍니다.", item.attack);
    //        else if (item.less_damage > 0)
    //            content = string.Format("피해를 {0} 줍니다.\n약화를 {1} 부여합니다.", item.attack, item.less_damage);
    //        else if (item.shield > 0)
    //            content = string.Format("방어도를 {0} 얻습니다.\n피해를 {0} 줍니다.", item.attack, item.shield);
    //        else if (item.attack_count > 1)
    //            content = string.Format("피해를 {0} 만큼 {1} 번 줍니다.", item.attack, item.attack_count);
    //        else
    //            content = string.Format("피해를 {0} 줍니다.", item.attack);
    //        if (item.draw_card > 0)
    //            content += string.Format("\n카드를 {0} 장 뽑습니다", item.draw_card);
    //    }
    //    else if(item.shield > 0)
    //    {
    //        if (item.draw_card > 0)
    //            content = string.Format("방어도를 {0} 얻습니다.\n카드를 {1}장 뽑습니다.", item.shield, item.draw_card);
    //        else
    //            content = string.Format("방어도를 {0} 얻습니다.", item.shield);
    //    }
    //    else if(item.force > 0)
    //        content = string.Format("힘을 {0} 얻습니다.\n턴이 끝날 때 힘을 {0} 잃습니다.", item.force);
    //    else if(item.copy)
    //        content = "카드 한장을 복사합니다.";
    //    else
    //        content = string.Format("약화를 {0} 부여합니다.", item.less_damage);
    //    return content;
    //}

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.parent.DOMove(prs.pos, dotweenTime);
            transform.parent.DORotateQuaternion(prs.rot, dotweenTime);
            transform.parent.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.parent.position = prs.pos;
            transform.parent.rotation = prs.rot;
            transform.parent.localScale = prs.scale;
        }
    }
    public void FinishCard()
    {
        parent.localScale = Vector3.one * 0.1f;
        //SetActiveChildObj(false);
    }

    public void SetActiveChildObj(bool active)
    {

    }

    void OnMouseOver()
    {
        CardManager.Inst.CardMouseOver(this);
    }

    void OnMouseExit()
    {
        CardManager.Inst.CardMouseExit(this);
    }

    void OnMouseDown()
    {
        CardManager.Inst.CardMouseDown();
    }

    void OnMouseUp()
    {
        CardManager.Inst.CardMouseUp();
    }

    public void Use(GameObject obj = null)
    {
        if (obj.tag == "Player")
            Player.Inst.hpbar.Sheld(final_Num + 1);
        else
        {
            Player.Inst.Attack();
            EnemyManager.Inst.enemys[0].Damage(final_Num == obj.GetComponent<Enemy>().weaknessNum ? final_Num + 1 : 1);
        }
    }
}
