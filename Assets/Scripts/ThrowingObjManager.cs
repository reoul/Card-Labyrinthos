using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObjManager : MonoBehaviour
{
    public static ThrowingObjManager Inst = null;

    public GameObject CardBackPrefab;
    public GameObject CardPiecePrefab;
    public GameObject NumCardPrefab;
    public GameObject QuestionCardPrefab;

    void Awake()
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

    public void CreateThrowingObj(THROWING_OBJ_TYPE type, Vector3 startPos, Vector3 endPos, IEnumerator enumerator = null, float moveTime = 1, int cnt = 1)
    {
        StartCoroutine(CreateThrowingObjCorutine(type, startPos, endPos, enumerator, moveTime, cnt));
    }

    IEnumerator CreateThrowingObjCorutine(THROWING_OBJ_TYPE type, Vector3 startPos, Vector3 endPos, IEnumerator enumerator123 = null, float moveTime = 1, int cnt = 1)
    {
        for (int i = 0; i < cnt; i++)
        {
            var throwingObj = Instantiate(GetThrowingObjPrefab(type), startPos, Quaternion.identity);
            throwingObj.transform.DOMove(endPos, moveTime).SetEase(Ease.InQuint).OnComplete(() =>
            {
                if (enumerator123 != null)
                    StartCoroutine(enumerator123);
                EnemyManager.Inst.enemys[0].Damage(1);
                Destroy(throwingObj);
            });
            yield return new WaitForSeconds(0.05f);
        }
    }

    GameObject GetThrowingObjPrefab(THROWING_OBJ_TYPE type)
    {
        switch (type)
        {
            case THROWING_OBJ_TYPE.CARDBACK:
                return CardBackPrefab;
            case THROWING_OBJ_TYPE.CARD_PIECE:
                return CardPiecePrefab;
            case THROWING_OBJ_TYPE.NUM_CARD:
                return NumCardPrefab;
            case THROWING_OBJ_TYPE.QUESTION_CARD:
                return QuestionCardPrefab;
        }
        return CardBackPrefab;
    }
}
