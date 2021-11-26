using System.Collections;
using UnityEngine;

public enum EffectObjType { HIT, SHELD, HEAL }

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
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    GameObject GetEffectObj(EffectObjType type)     //이펙트 타입에 따른 오브젝트 반환
    {
        switch (type)
        {
            case EffectObjType.HIT:
                return this.hitObj;
            case EffectObjType.SHELD:
                return this.sheldObj;
            case EffectObjType.HEAL:
                return this.healObj;
            default:
                return this.hitObj;
        }
    }

    public void CreateEffectObj(EffectObjType type, Vector3 pos, float delay = 0, float destoryTime = 1, int cnt = 1)
    {
        this.StartCoroutine(this.DelayAndMultipleCreate(type, pos, delay, destoryTime, cnt));
    }

    void CreateObjAndDestory(EffectObjType type, Vector3 pos, float destoryTime = 1)
    {
        GameObject obj = Instantiate(this.GetEffectObj(type), pos, Quaternion.identity);
        obj.GetComponent<EffectObj>().Init(type);
        Destroy(obj, destoryTime);
    }

    private IEnumerator DelayAndMultipleCreate(EffectObjType type, Vector3 pos, float delay = 0, float destoryTime = 1, int cnt = 1, float multipleDelay = 0.07f)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < cnt; i++)
        {
            this.CreateObjAndDestory(type, pos, destoryTime);
            yield return new WaitForSeconds(multipleDelay);
        }
    }

    private IEnumerator CreateSheldCoroutine(Vector3 pos, float delay, int cnt)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < cnt; i++)
        {
            GameObject obj = Instantiate(this.hitObj, pos + new Vector3(0, 0, -15), Quaternion.identity);
            Destroy(obj, 1);
            yield return new WaitForSeconds(0.07f);
        }
    }

    private IEnumerator CreateHealCoroutine(Vector3 pos, float delay, int cnt)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < cnt; i++)
        {
            GameObject obj = Instantiate(this.hitObj, pos + new Vector3(0, 0, -15), Quaternion.identity);
            Destroy(obj, 1);
            yield return new WaitForSeconds(0.07f);
        }
    }
}
