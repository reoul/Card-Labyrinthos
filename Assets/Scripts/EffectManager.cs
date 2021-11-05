using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Inst = null;

    public GameObject hitObj;

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

    public void CreateHitObj(Vector3 pos, float delay, int cnt)
    {
        StartCoroutine(CreateHitObjCorutine(pos, delay, cnt));
    }

    private IEnumerator CreateHitObjCorutine(Vector3 pos, float delay, int cnt)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < cnt; i++)
        {
            GameObject obj = GameObject.Instantiate(hitObj, pos + new Vector3(0, 0, -15), Quaternion.identity);
            Destroy(obj, 1);
            yield return new WaitForSeconds(0.07f);
        }
    }
}
