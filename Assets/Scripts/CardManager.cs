using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum THROWING_OBJ_TYPE { CARDBACK, CARD_PIECE, NUM_CARD, QUESTION_CARD, SKILL_BOOK }

public class CardManager : MonoBehaviour
{
    public bool isIntro;
    public static CardManager Inst = null;
    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            if (!isIntro)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //[SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject beenCardPrefab;
    [SerializeField] Transform cardSpawnPoint;  //뽑기 카드 더미 위치
    [SerializeField] Transform cardEndPoint;  //버린 카드 더미 위치
    [SerializeField] Transform myCardLeft;  //내 손패 왼쪽 포지션
    [SerializeField] Transform myCardRight; //내 손패 오른쪽 포지션
    [SerializeField] Transform CardCenterPoint;

    public int[] cardDeck;        //현재 플레이어 카드 덱, 1~6
    public int[] fixedCardNum;      //카드 숫자 고정 시킬때 사용 -1이면 고정 안함

    public GameObject IntroCardPrefab;

    public List<Card> MyHandCards;    //내 손에 들고 있는 카드 리스트
    [SerializeField] List<Card> itemBuffer;  //뽑을 카드 더미
    [SerializeField] List<Card> tombItemBuffer;  //버린 카드 더미, 사용한 카드가 여기 리스트에 쌓인다

    public Card selectCard;     //선택된 카드

    [SerializeField] bool isMyCardDrag;          //현재 플레이어가 카드를 드래그 중인지
    [SerializeField] bool onMyCardArea;          //플레이어 마우스가 카드Area에 있는지

    public Ease ease;

    public Transform waypoint2;

    private Vector3[] waypoints;        //카드 사용후 버린 카드더미로 이동할때 사용

    [Header("카드의 이동속도")]
    public float CardMoveSpeed;

    public bool isTutorial = false;

    public int HandCardNumSum
    {
        get
        {
            int sum = 0;
            for (int i = 0; i < MyHandCards.Count; i++)
            {
                sum += MyHandCards[i].final_Num + 1;
            }
            return sum;
        }
    }

    //Quaternion cardRotate = Utils.QI;

    private void Start()
    {
        fixedCardNum = new int[12];
        for (int i = 0; i < fixedCardNum.Length; i++)
        {
            fixedCardNum[i] = -1;
        }
    }

    void Update()
    {
        if (MapManager.Inst.CurrentSceneName != "지도" && MapManager.Inst.CurrentSceneName != "상점")
        {
            if (isMyCardDrag)
                CardDrag();
            DetectCardArea();
        }
    }

    public Card PopItem()   //카드 뽑기
    {
        if (itemBuffer.Count == 0)
            CardTombToItemBuffer();

        Card card = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return card;
    }

    void SetupItemBuffer()  //초기 카드 생성
    {
        itemBuffer = new List<Card>();
        tombItemBuffer = new List<Card>();
        MyHandCards = new List<Card>();
        for (int i = 0; i < cardDeck.Length; i++)
        {
            for (int j = 0; j < cardDeck[i]; j++)
            {
                GameObject cardObj = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.CardRotate);
                Card card = cardObj.GetComponentInChildren<Card>();
                cardObj.transform.localScale = Vector3.zero;
                cardObj.name = (i + 1).ToString();
                cardObj.gameObject.SetActive(false);
                card.Setup(i);
                itemBuffer.Add(card);
            }
        }
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Card temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
        for (int i = 0; i < fixedCardNum.Length; i++)
        {
            if (fixedCardNum[i] != -1)
            {
                itemBuffer[i].SetFinalNum(fixedCardNum[i]);
                fixedCardNum[i] = -1;
            }
        }
    }

    void ShuffleCard()      //버린 카드 더미를 섞는다
    {
        SoundManager.Inst.Play(CARDSOUND.Shuffling);
        for (int i = 0; i < tombItemBuffer.Count; i++)
        {
            int rand = Random.Range(i, tombItemBuffer.Count);
            Card temp = tombItemBuffer[i];
            tombItemBuffer[i] = tombItemBuffer[rand];
            tombItemBuffer[rand] = temp;
            tombItemBuffer[i].RevertOriginNum();
        }
    }

    public IEnumerator InitCoroutine()
    {
        cardSpawnPoint = GameObject.Find("CardSpawn").transform;
        cardEndPoint = GameObject.Find("CardEnd").transform;
        myCardLeft = GameObject.Find("CardLeft").transform;
        myCardRight = GameObject.Find("CardRight").transform;
        waypoint2 = GameObject.Find("WayPoint").transform;

        SetupItemBuffer();
        if (TurnManager.OnAddCard != null)
            TurnManager.OnAddCard -= AddCard;
        TurnManager.OnAddCard += AddCard;

        InitCard();
        yield return null;
    }

    void InitCard()         //카드 초기화
    {
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            itemBuffer[i].parent.position = cardSpawnPoint.position;
            itemBuffer[i].parent.rotation = Utils.CardRotate;
            itemBuffer[i].parent.localScale = Vector3.zero;
        }
    }

    public void FinishBattle()          //전투가 끝날을때 호출
    {
        Init();
        TurnManager.OnAddCard -= AddCard;
        RewardManager.Inst.SetFinishBattleReward();
        StartCoroutine(TurnManager.Inst.ShowReward());
    }

    public void Init()
    {
        while (MyHandCards.Count > 0)
        {
            GameObject card = MyHandCards[0].parent.gameObject;
            MyHandCards.RemoveAt(0);
            Destroy(card);
        }
        while (itemBuffer.Count > 0)
        {
            GameObject card = itemBuffer[0].parent.gameObject;
            itemBuffer.RemoveAt(0);
            Destroy(card);
        }
        while (tombItemBuffer.Count > 0)
        {
            GameObject card = tombItemBuffer[0].parent.gameObject;
            tombItemBuffer.RemoveAt(0);
            Destroy(card);
        }
        selectCard = null;
        MyHandCards = null;
        itemBuffer = null;
        tombItemBuffer = null;
    }

    public void CardTombToItemBuffer()      //버린 카드 더미에서 뽑을 카드 더미로 섞고 이동
    {
        ShuffleCard();
        //StartCoroutine(CreateBeenCard());
        for (int i = 0; i < tombItemBuffer.Count; i++)
        {
            itemBuffer.Add(tombItemBuffer[i]);
        }
        for (int i = 0; i < tombItemBuffer.Count;)
        {
            tombItemBuffer.RemoveAt(0);
        }
        InitCard();
    }


    void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
    }


    public void TurnStartDraw()
    {
        StartCoroutine(DrawCardCoroutine(5));
    }

    public IEnumerator DrawCardCoroutine(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            AddCard();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SelectCardNumAdd(int index)
    {
        if (selectCard != null)
        {
            selectCard.AddNum(index);
        }
    }

    public void AddCard()           //카드 추가(카드 드로우시 사용)
    {
        SoundManager.Inst.Play(BATTLESOUND.CARD_DRAW);
        Card card = PopItem();
        card.parent.gameObject.SetActive(true);
        card.SetActiveChildObj(true);
        MyHandCards.Add(card);

        SetOriginOrder();
        CardAlignment();
    }

    void SetOriginOrder()           //카드 랜더링 순서 조정
    {
        int count = MyHandCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = MyHandCards[i];
            targetCard.GetComponent<Order>().SetOriginOrder(3700 + i * 10);
        }
    }

    void CardAlignment()            //카드 위치 조정
    {
        List<PRS> originCardPRSs = new List<PRS>();

        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, MyHandCards.Count, 0.5f, Vector3.one);
        for (int i = 0; i < MyHandCards.Count; i++)
        {
            var targetCard = MyHandCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, CardMoveSpeed);
        }
    }

    public void FnishCardAllMyHand()
    {
        StartCoroutine(FinishTurnCoroutine());
    }

    public void FinishSceneAllMyHand()      //씬이 끌날때 손에 있는 모든 카드를 밑으로 내려버린다.
    {
        if (selectCard != null)
            isMyCardDrag = false;
        selectCard = null;
        for (int i = 0; i < MyHandCards.Count; i++)
        {
            MyHandCards[i].FinishScene();
        }
    }

    IEnumerator FinishTurnCoroutine()
    {
        for (int i = 0; i < MyHandCards.Count; i++)
        {
            MyHandCards[i].FinishCard();
        }
        for (int i = 0; i < MyHandCards.Count; i++)
        {
            yield return new WaitForEndOfFrame();
            PRS endPRS = new PRS(cardEndPoint.position, Utils.CardRotate, Vector3.one * 0.1f);
            var card = MyHandCards[i].GetComponent<Card>();
            card.transform.DOMove(cardEndPoint.position, 0.7f).OnComplete(() =>
            {
                MyHandCards.Remove(card);
                Destroy(card.gameObject);
            });
            tombItemBuffer.Add(card);
        }
        //yield return new WaitForSeconds(1.5f);
        //for (int i = 0; i < MyHandCards.Count;)
        //{
        //    GameObject target = MyHandCards[i].gameObject;
        //    MyHandCards.Remove(MyHandCards[i]);
        //    Destroy(target);
        //}
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)        //카드 정렬(둥글게 정렬)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1:
                objLerps = new float[] { 0.5f };
                break;
            case 2:
                objLerps = new float[] { 0.4f, 0.6f };
                break;
            case 3:
                objLerps = new float[] { 0.3f, 0.5f, 0.7f };
                break;
            case 4:
                objLerps = new float[] { 0.2f, 0.4f, 0.6f, 0.8f };
                break;
            case 5:
                objLerps = new float[] { 0.1f, 0.3f, 0.5f, 0.7f, 0.9f };
                break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.CardRotate;

            float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
            targetPos.y += curve;
            targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);

            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    public void CardMouseOver(Card card)
    {
        if (!isMyCardDrag)
            selectCard = card;
        if (selectCard == card && onMyCardArea)
        {
            EnlargeCard(true, card);
        }
    }

    public void CardMouseExit(Card card)
    {
        EnlargeCard(false, card);
    }

    public void CardMouseDown()
    {
        if (onMyCardArea)
        {
            SoundManager.Inst.Play(CARDSOUND.UP_CARD);
            isMyCardDrag = true;
        }
    }

    public void CardMouseUp()
    {
        if (isIntro)
        {
            isMyCardDrag = false;
            EnlargeCard(false, selectCard);
            switch (selectCard.final_Num)
            {
                case 0:
                    MapManager.Inst.LoadTutorialScene();
                    break;
                case 1: //옵션창
                    break;
                case 2:     //게임 종료
                    Application.Quit();
                    break;
                default:
                    break;
            }
            return;
        }
        isMyCardDrag = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);

        int layer = LayerMask.NameToLayer("SkillBookCard");
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.layer == layer)
            {
                hits[i].collider.GetComponent<SkillBookCard>().SetCard(selectCard);
                EnlargeCard(false, selectCard, false);
            }
        }

        layer = LayerMask.NameToLayer("Player");
        bool isUse = false;
        if (Array.Exists(hits, x => x.collider.gameObject.layer == layer) && hits.Length <= 2)      //만약 플레이어라면
        {
            EnlargeCard(false, selectCard, true);
            isUse = true;
            UseCard(Player.Inst.gameObject);

            if (isTutorial)
            {
                TalkWindow.Inst.SetFlagIndex(false);
                TalkWindow.Inst.SetFlagNext(true);
                TalkWindow.Inst.SetSkip(true);
            }

        }
        else
        {
            layer = LayerMask.NameToLayer("Enemy");
            if (Array.Exists(hits, x => x.collider.gameObject.layer == layer) && hits.Length <= 2)      //만약 적이라면
            {
                EnlargeCard(false, selectCard, true);

                isUse = true;

                int damage = selectCard.final_Num == EnemyManager.Inst.enemys[0].GetComponent<Enemy>().weaknessNum ? selectCard.final_Num + 1 : 1;

                UseCard(EnemyManager.Inst.enemys[0].gameObject);

                ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.CARDBACK,
                    Player.Inst.gameObject.transform.position + Vector3.up * 3.5f, EnemyManager.Inst.enemys[0].hitPos.position, null, 0.5f, damage);

                EffectManager.Inst.CreateEffectObj(EffectObjType.HIT, EnemyManager.Inst.enemys[0].hitPos.position + new Vector3(0, 0, -15), 0.2f, 1, damage);

                if (isTutorial)
                {
                    TalkWindow.Inst.SetFlagIndex(false);
                    TalkWindow.Inst.SetFlagNext(true);
                    TalkWindow.Inst.SetSkip(true);
                }

            }
        }

        if (MyHandCards.Count == 0)
            TurnManager.Inst.EndTurn();

        if (!isUse)
            EnlargeCard(false, selectCard);
    }

    void UseCard(GameObject obj)        //카드 사용
    {
        tombItemBuffer.Add(selectCard);
        MyHandCards.Remove(selectCard);
        CardAlignment();
        selectCard.parent.position = new Vector3(selectCard.parent.position.x, selectCard.parent.position.y, -3);
        selectCard.originPRS.pos = selectCard.parent.position;
        selectCard.originPRS.scale = Vector3.one * 0.1f;
        selectCard.Use(obj);
        CardFinishMove();
    }

    IEnumerator CreateBeenCard()
    {
        for (int i = 0; i < tombItemBuffer.Count; i++)
        {
            var cardObject = Instantiate(beenCardPrefab, cardEndPoint.position, Utils.CardRotate);
            cardObject.transform.DOMove(cardSpawnPoint.position, 1f).OnComplete(() =>
            {
                Destroy(cardObject);
            });
            yield return new WaitForEndOfFrame();
        }
    }

    void CardDrag()             //카드 드래그
    {
        if (!onMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.CardRotate, selectCard.originPRS.scale * 0.5f), false);
        }
        else
        {
            EnlargeCard(true, selectCard);
        }
    }

    public void CardFinishMove()            //카드 사용 후 버린 카드 더미로 이동
    {
        selectCard.FinishCard();
        selectCard.GetComponent<Order>().SetOriginOrder(3700);
        waypoints = new Vector3[2];
        waypoints.SetValue(selectCard.parent.position, 0);
        waypoints.SetValue(waypoint2.position, 0);
        waypoints.SetValue(cardEndPoint.position, 1);
        GameObject target = selectCard.parent.gameObject;
        target.transform.DOPath(waypoints, 1, PathType.CatmullRom).
            SetLookAt(cardEndPoint).SetEase(ease).OnComplete(() =>
            {
                target.SetActive(false);
            });
        selectCard = null;
    }

    public IEnumerator MoveCenterCardCoroutine(Card card)
    {
        card.parent.DORotateQuaternion(Utils.CardRotate, 0.5f);
        card.parent.DOMove(CardCenterPoint.position, 0.5f).OnComplete(() =>
        {
            CardFinishMove();
        });
        yield return null;
    }

    void EnlargeCard(bool isEnlarge, Card card, bool isUse = false)         //카드 확대 함수
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, onMyCardArea ? -4 : card.originPRS.pos.y, onMyCardArea ? -100 : card.originPRS.pos.z);
            card.MoveTransform(new PRS(enlargePos, Utils.CardRotate, Vector3.one * (onMyCardArea ? 1.5f : 1)), false);
        }
        else
        {
            if (isUse)
                card.MoveTransform(new PRS(card.parent.transform.position, card.parent.transform.rotation, Vector3.one), false);
            else
                card.MoveTransform(card.originPRS, false);
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void DetectCardArea()           //마우스가 카드area에 있는지 체크
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    public void AddCardDeck(Card card)      //플레이어 카드 덱에 카드 추가, 상점이나 보상에서 카드 획득할 때
    {
        cardDeck[card.original_Num]++;
    }

    public void AddCardDeck(int card, int index = 1)      //플레이어 카드 덱에 카드 추가, 상점이나 보상에서 카드 획득할 때
    {
        if (!isCardDeckMax()[card])
            cardDeck[card] += index;
    }

    public bool[] isCardDeckMax()       //카드덱에 최대치가 된 카드가 있는지
    {
        bool[] check = new bool[6];
        for (int i = 0; i < check.Length; i++)
        {
            check[i] = cardDeck[i] >= (18 - 3 * i);
        }
        return check;
    }

    public IEnumerator StartIntroCard()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            Card card = Instantiate(IntroCardPrefab, cardSpawnPoint.position, Utils.CardRotate).GetComponentInChildren<Card>();
            card.transform.parent.localScale = Vector3.zero;
            switch (i)
            {
                case 0:
                    card.Setup(i);
                    card.num_TMP.text = "게임시작";
                    break;
                case 1:
                    card.Setup(i);
                    card.num_TMP.text = "옵션";
                    break;
                case 2:
                    card.Setup(i);
                    card.num_TMP.text = "게임종료";
                    break;
                default:
                    break;
            }
            MyHandCards.Add(card.GetComponentInChildren<Card>());
            SetOriginOrder();
            CardAlignment();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public IEnumerator FixedCardNumToturial2Coroutine()          //첫 전투 튜토리얼때 카드 숫자 고정
    {
        fixedCardNum[0] = 2;
        fixedCardNum[1] = 5;
        fixedCardNum[2] = 3;

        fixedCardNum[3] = 0;
        fixedCardNum[4] = 1;
        fixedCardNum[5] = 4;

        fixedCardNum[6] = 4;
        fixedCardNum[7] = 3;
        fixedCardNum[8] = 2;

        fixedCardNum[9] = 2;
        fixedCardNum[10] = 1;
        fixedCardNum[11] = 3;
        yield return null;
    }

    public void LockMyHandCard(int index)
    {
        if (index < MyHandCards.Count)
            MyHandCards[index].Lock();
    }

    public void UnLockMyHandCard(int index)
    {
        if (index < MyHandCards.Count)
            MyHandCards[index].UnLock();
    }

    public void LockMyHandCardAll()
    {
        for (int i = 0; i < MyHandCards.Count; i++)
        {
            LockMyHandCard(i);
        }
    }

    public void UnLockMyHandCardAll()
    {
        for (int i = 0; i < MyHandCards.Count; i++)
        {
            UnLockMyHandCard(i);
        }
    }
}
