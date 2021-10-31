using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObjManager : MonoBehaviour
{
    public static ThrowingObjManager Inst = null;

    public List<GameObject> throwingRewardObj;

    public int moveThrowingReward { get { return throwingRewardObj.Count; } }

    public GameObject CardBackPrefab;
    public GameObject CardPiecePrefab;
    public GameObject NumCardPrefab;
    public GameObject QuestionCardPrefab;
    public GameObject SkillBookPrefab;

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

    private void Start()
    {
        throwingRewardObj = new List<GameObject>();
    }

    public void CreateThrowingObj(THROWING_OBJ_TYPE type, Vector3 startPos, Vector3 endPos, IEnumerator enumerator = null, float moveTime = 1, int cnt = 1, int index = 0)   //index : 추가적으로 변수를 함수로 넘겨줘야할 경우
    {
        StartCoroutine(CreateThrowingObjCorutine(type, startPos, endPos, enumerator, moveTime, cnt, index));
    }

    IEnumerator CreateThrowingObjCorutine(THROWING_OBJ_TYPE type, Vector3 startPos, Vector3 endPos, IEnumerator enumerator123 = null, float moveTime = 1, int cnt = 1, int index = 0)
    {
        for (int i = 0; i < cnt; i++)
        {
            var throwingObj = Instantiate(GetThrowingObjPrefab(type), startPos, type == THROWING_OBJ_TYPE.NUM_CARD ? Utils.CardRotate : Quaternion.identity);
            throwingRewardObj.Add(throwingObj);
            if (type == THROWING_OBJ_TYPE.NUM_CARD)
                throwingObj.GetComponentInChildren<Card>().Setup(index);
            throwingObj.transform.DOMove(endPos, moveTime).SetEase(Ease.InQuint).OnComplete(() =>
            {
                if (enumerator123 != null)
                    StartCoroutine(enumerator123);
                switch (type)
                {
                    case THROWING_OBJ_TYPE.CARDBACK:
                        break;
                    case THROWING_OBJ_TYPE.CARD_PIECE:
                        PlayerManager.Inst.card_piece += index;
                        break;
                    case THROWING_OBJ_TYPE.NUM_CARD:
                        break;
                    case THROWING_OBJ_TYPE.QUESTION_CARD:
                        PlayerManager.Inst.question_card += index;
                        break;
                }
                if (EnemyManager.Inst.enemys.Count > 0)
                    EnemyManager.Inst.enemys[0].Damage(1);
                throwingRewardObj.Remove(throwingObj);
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
            case THROWING_OBJ_TYPE.SKILL_BOOK:
                return SkillBookPrefab;
        }
        return CardBackPrefab;
    }
}
