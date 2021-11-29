using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ThrowingObjManager : MonoBehaviour
{
    public static ThrowingObjManager Inst;

    public List<GameObject> throwingRewardObj;

    public int moveThrowingReward { get { return throwingRewardObj.Count; } }

    public GameObject CardBackPrefab;
    public GameObject CardPiecePrefab;
    public GameObject NumCardPrefab;
    public GameObject QuestionCardPrefab;
    public GameObject SkillBookPrefab;

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

    private void Start()
    {
        throwingRewardObj = new List<GameObject>();
    }

    public void CreateThrowingObj(THROWING_OBJ_TYPE type, Vector3 startPos, Vector3 endPos, IEnumerator enumerator = null, float moveTime = 1, int cnt = 1, int index = 0)   //index : 추가적으로 변수를 함수로 넘겨줘야할 경우
    {
        StartCoroutine(CreateThrowingObjCoroutine(type, startPos, endPos, enumerator, moveTime, cnt, index));
    }

    private IEnumerator CreateThrowingObjCoroutine(THROWING_OBJ_TYPE type, Vector3 startPos, Vector3 endPos, IEnumerator enumerator123 = null, float moveTime = 1, int cnt = 1, int index = 0)
    {
        for (int i = 0; i < cnt; i++)
        {
            SoundManager.Inst.Play(SHOPSOUND.ThrowingObj);
            var throwingObj = Instantiate(GetThrowingObjPrefab(type), startPos, type == THROWING_OBJ_TYPE.NumCard ? Utils.CardRotate : Quaternion.identity);
            throwingRewardObj.Add(throwingObj);
            if (type == THROWING_OBJ_TYPE.NumCard)
            {
                throwingObj.GetComponentInChildren<Card>().Setup(index);
                throwingObj.GetComponentInChildren<Card>().SetOrderLayer(5500 + i * 4);
            }
            throwingObj.transform.DOMove(endPos, moveTime).SetEase(Ease.InQuint).OnComplete(() =>
            {
                if (enumerator123 != null)
                {
                    StartCoroutine(enumerator123);
                }

                switch (type)
                {
                    case THROWING_OBJ_TYPE.CardBack:
                        break;
                    case THROWING_OBJ_TYPE.CardPiece:
                        SoundManager.Inst.Play(REWARDSOUND.GetCardPiece);
                        PlayerManager.Inst.CardPiece += index;
                        break;
                    case THROWING_OBJ_TYPE.NumCard:
                        break;
                    case THROWING_OBJ_TYPE.QuestionCard:
                        SoundManager.Inst.Play(REWARDSOUND.GetQuestion);
                        PlayerManager.Inst.QuestionCard += index;
                        break;
                }
                if (EnemyManager.Inst.enemys.Count > 0)
                {
                    EnemyManager.Inst.enemys[0].Damage(1);
                }

                throwingRewardObj.Remove(throwingObj);
                Destroy(throwingObj);
            });
            yield return new WaitForSeconds(0.07f);
        }
    }

    private GameObject GetThrowingObjPrefab(THROWING_OBJ_TYPE type)
    {
        switch (type)
        {
            case THROWING_OBJ_TYPE.CardBack:
                return CardBackPrefab;
            case THROWING_OBJ_TYPE.CardPiece:
                return CardPiecePrefab;
            case THROWING_OBJ_TYPE.NumCard:
                return NumCardPrefab;
            case THROWING_OBJ_TYPE.QuestionCard:
                return QuestionCardPrefab;
            case THROWING_OBJ_TYPE.SkillBook:
                return SkillBookPrefab;
        }
        return CardBackPrefab;
    }
}
