using System.Collections;
using UnityEngine;

public enum EffectObjType
{
    Hit,
    Sheld,
    Heal
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager Inst;

    public GameObject hitObj;
    public GameObject sheldObj;
    public GameObject healObj;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private GameObject GetEffectObj(EffectObjType type) //이펙트 타입에 따른 오브젝트 반환
    {
        switch (type)
        {
            case EffectObjType.Hit: return hitObj;
            case EffectObjType.Sheld: return sheldObj;
            case EffectObjType.Heal: return healObj;
            default: return hitObj;
        }
    }

    public void CreateEffectObj(EffectObjType type, Vector3 pos, float delay = 0, float destoryTime = 1, int cnt = 1)
    {
        StartCoroutine(DelayAndMultipleCreate(type, pos, delay, destoryTime, cnt));
    }

    private void CreateObjAndDestory(EffectObjType type, Vector3 pos, float destoryTime = 1)
    {
        GameObject obj = Instantiate(GetEffectObj(type), pos, Quaternion.identity);
        obj.GetComponent<EffectObj>().Init(type);
        Destroy(obj, destoryTime);
    }

    private IEnumerator DelayAndMultipleCreate(EffectObjType type, Vector3 pos, float delay = 0, float destoryTime = 1,
        int cnt = 1, float multipleDelay = 0.07f)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < cnt; i++)
        {
            CreateObjAndDestory(type, pos, destoryTime);
            yield return new WaitForSeconds(multipleDelay);
        }
    }
}
