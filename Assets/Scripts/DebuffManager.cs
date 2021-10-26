using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager Inst = null;

    public DEBUFF_TYPE debuff_type;

    public string DebuffString
    {
        get
        {
            switch (debuff_type)
            {
                case DEBUFF_TYPE.DEBUFF1: return "매턴마다 데미지 1이상 못 넣었을때 플레이어에게 데미지를 2만큼 입힙니다";
                case DEBUFF_TYPE.DEBUFF2: return "매턴마다 데미지 1이상 못 넣었을때 몬스터의 체력이 2만큼 회복됩니다";
                case DEBUFF_TYPE.DEBUFF3: return "매턴마다 데미지 1이상 못 넣었을때 몬스터의 다음 약점숫자를 알 수 없게 됩니다";
                case DEBUFF_TYPE.DEBUFF4: return "매턴마다 데미지 1이상 못 넣었을때 몬스터의 다음 패턴을 알 수 없게 됩니다";
                case DEBUFF_TYPE.DEBUFF5: return "2턴마다 몬스터의 데미지가 1만큼 증가합니다";
                case DEBUFF_TYPE.DEBUFF6: return "몬스터가 플레이어에게 넣은 피해만큼 회복합니다";
                case DEBUFF_TYPE.DEBUFF7: return "매턴마다 몬스터의 방어도가 3씩 쌓입니다";
            }
            debuff_type = DEBUFF_TYPE.DEBUFF1;
            return "매턴마다 데미지 1이상 못넣었을때 플레이어에게 데미지를 2만큼 입힙니다";
        }
    }

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
}
