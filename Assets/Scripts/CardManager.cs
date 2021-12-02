using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum THROWING_OBJ_TYPE
{
    CardBack,
    CardPiece,
    NumCard,
    QuestionCard,
    SkillBook
}

public class CardManager : Singleton<CardManager>
{
    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);
    }

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject beenCardPrefab;
    [SerializeField] private Transform cardSpawnPoint; //뽑기 카드 더미 위치
    [SerializeField] private Transform cardEndPoint; //버린 카드 더미 위치
    [SerializeField] private Transform myCardLeft; //내 손패 왼쪽 포지션
    [SerializeField] private Transform myCardRight; //내 손패 오른쪽 포지션

    public int[] cardDeck; //현재 플레이어 카드 덱, 1~6
    public int[] fixedCardNum; //카드 숫자 고정 시킬때 사용 -1이면 고정 안함

    public List<Card> MyHandCards; //내 손에 들고 있는 카드 리스트
    [SerializeField] private List<Card> itemBuffer; //뽑을 카드 더미
    [SerializeField] private List<Card> tombItemBuffer; //버린 카드 더미, 사용한 카드가 여기 리스트에 쌓인다

    public Card selectCard; //선택된 카드

    [SerializeField] private bool isMyCardDrag; //현재 플레이어가 카드를 드래그 중인지
    [SerializeField] private bool onMyCardArea; //플레이어 마우스가 카드Area에 있는지

    public Ease ease;

    public Transform waypoint2;

    private Vector3[] waypoints; //카드 사용후 버린 카드더미로 이동할때 사용

    [Header("카드의 이동속도")] public float CardMoveSpeed;

    public bool isTutorial;

    public int HandCardNumSum
    {
        get
        {
            var sum = 0;
            foreach (Card card in MyHandCards)
            {
                sum += card.FinalNum + 1;
            }

            return sum;
        }
    }

    private void Start()
    {
        fixedCardNum = new int[12];
        for (var i = 0; i < fixedCardNum.Length; i++)
        {
            fixedCardNum[i] = -1;
        }
    }

    private void Update()
    {
        if (MapManager.CurrentSceneName == "지도" || MapManager.CurrentSceneName == "상점")
        {
            return;
        }

        if (isMyCardDrag)
        {
            CardDrag();
        }

        DetectCardArea();
    }

    /// <summary>
    /// 카드 뽑기
    /// </summary>
    private Card PopItem()
    {
        if (itemBuffer.Count == 0)
        {
            CardTombToItemBuffer();
        }

        Card card = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return card;
    }

    /// <summary>
    /// 초기 카드 생성
    /// </summary>
    private void SetupItemBuffer()
    {
        itemBuffer = new List<Card>();
        tombItemBuffer = new List<Card>();
        MyHandCards = new List<Card>();
        for (var i = 0; i < cardDeck.Length; i++)
        {
            for (var j = 0; j < cardDeck[i]; j++)
            {
                var cardObj = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.CardRotate);
                var card = cardObj.GetComponentInChildren<Card>();
                cardObj.transform.localScale = Vector3.zero;
                cardObj.name = (i + 1).ToString();
                cardObj.gameObject.SetActive(false);
                card.Setup(i);
                itemBuffer.Add(card);
            }
        }

        for (var i = 0; i < itemBuffer.Count; i++)
        {
            var rand = Random.Range(i, itemBuffer.Count);
            (itemBuffer[i], itemBuffer[rand]) = (itemBuffer[rand], itemBuffer[i]);
        }

        for (var i = 0; i < fixedCardNum.Length; i++)
        {
            if (fixedCardNum[i] != -1)
            {
                itemBuffer[i].SetFinalNum(fixedCardNum[i]);
                fixedCardNum[i] = -1;
            }
        }
    }

    /// <summary>
    /// 버린 카드 더미를 섞는다
    /// </summary>
    private void ShuffleCard()
    {
        SoundManager.Inst.Play(CARDSOUND.Shuffling);
        for (var i = 0; i < tombItemBuffer.Count; i++)
        {
            var rand = Random.Range(i, tombItemBuffer.Count);
            (tombItemBuffer[i], tombItemBuffer[rand]) = (tombItemBuffer[rand], tombItemBuffer[i]);
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
        {
            TurnManager.OnAddCard -= AddCard;
        }

        TurnManager.OnAddCard += AddCard;

        InitCard();
        yield return null;
    }

    private void InitCard() //카드 초기화
    {
        foreach (Card card in itemBuffer)
        {
            card.Parent.position = cardSpawnPoint.position;
            card.Parent.rotation = Utils.CardRotate;
            card.Parent.localScale = Vector3.zero;
        }
    }

    public void Init()
    {
        while (MyHandCards.Count > 0)
        {
            var card = MyHandCards[0].Parent.gameObject;
            MyHandCards.RemoveAt(0);
            Destroy(card);
        }

        while (itemBuffer.Count > 0)
        {
            var card = itemBuffer[0].Parent.gameObject;
            itemBuffer.RemoveAt(0);
            Destroy(card);
        }

        while (tombItemBuffer.Count > 0)
        {
            var card = tombItemBuffer[0].Parent.gameObject;
            tombItemBuffer.RemoveAt(0);
            Destroy(card);
        }

        selectCard = null;
        MyHandCards = null;
        itemBuffer = null;
        tombItemBuffer = null;
    }

    private void CardTombToItemBuffer() //버린 카드 더미에서 뽑을 카드 더미로 섞고 이동
    {
        ShuffleCard();

        foreach (Card card in tombItemBuffer)
        {
            itemBuffer.Add(card);
        }

        for (var i = 0; i < tombItemBuffer.Count;)
        {
            tombItemBuffer.RemoveAt(0);
        }

        InitCard();
    }

    private void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
    }

    public void AddCard() //카드 추가(카드 드로우시 사용)
    {
        SoundManager.Inst.Play(BATTLESOUND.CardDraw);
        Card card = PopItem();
        card.Parent.gameObject.SetActive(true);
        card.SetActiveChildObj(true);
        MyHandCards.Add(card);

        SetOriginOrder();
        CardAlignment();
    }

    private void SetOriginOrder() //카드 랜더링 순서 조정
    {
        int count = MyHandCards.Count;
        for (var i = 0; i < count; i++)
        {
            var targetCard = MyHandCards[i];
            targetCard.GetComponent<Order>().SetOriginOrder(3700 + i * 10);
        }
    }

    private void CardAlignment() //카드 위치 조정
    {
        List<PRS> originCardPRSs =
            RoundAlignment(myCardLeft, myCardRight, MyHandCards.Count, Vector3.one);

        for (var i = 0; i < MyHandCards.Count; i++)
        {
            var targetCard = MyHandCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, CardMoveSpeed);
        }
    }

    public void FinishSceneAllMyHand() //씬이 끌날때 손에 있는 모든 카드를 밑으로 내려버린다.
    {
        if (selectCard != null)
        {
            isMyCardDrag = false;
        }

        selectCard = null;
        foreach (Card card in MyHandCards)
        {
            card.FinishScene();
        }
    }

    // 카드 정렬 함수
    private List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, Vector3 scale,
        float height = 0.5f)
    {
        var objLerps = new float[objCount];
        var results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1:
                objLerps = new[] {0.5f};
                break;
            case 2:
                objLerps = new[] {0.4f, 0.6f};
                break;
            case 3:
                objLerps = new[] {0.3f, 0.5f, 0.7f};
                break;
            case 4:
                objLerps = new[] {0.2f, 0.4f, 0.6f, 0.8f};
                break;
            case 5:
                objLerps = new[] {0.1f, 0.3f, 0.5f, 0.7f, 0.9f};
                break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }

                break;
        }

        for (var i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);

            var curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
            targetPos.y += curve;
            var targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);

            results.Add(new PRS(targetPos, targetRot, scale));
        }

        return results;
    }

    public void CardMouseOver(Card card)
    {
        if (!isMyCardDrag)
        {
            selectCard = card;
        }

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
        if (!onMyCardArea)
        {
            return;
        }

        SoundManager.Inst.Play(CARDSOUND.UpCard);
        isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);

        int layer = LayerMask.NameToLayer("SkillBookCard");
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == layer && SkillManager.Inst.ActivePage.isFinishFade)
            {
                hit.collider.GetComponent<SkillBookCard>().SetCard(selectCard);
                EnlargeCard(false, selectCard);
            }
        }

        layer = LayerMask.NameToLayer("Player");
        bool isUse = false;
        if (Array.Exists(hits, x => x.collider.gameObject.layer == layer) && hits.Length <= 2) //만약 플레이어라면
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
            if (Array.Exists(hits, x => x.collider.gameObject.layer == layer) && hits.Length <= 2) //만약 적이라면
            {
                EnlargeCard(false, selectCard, true);

                isUse = true;

                int damage = selectCard.FinalNum == EnemyManager.Inst.enemys[0].GetComponent<Enemy>().weaknessNum
                    ? selectCard.FinalNum + 1
                    : 1;

                UseCard(EnemyManager.Inst.enemys[0].gameObject);

                ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.CardBack,
                    Player.Inst.gameObject.transform.position + Vector3.up * 3.5f,
                    EnemyManager.Inst.enemys[0].hitPos.position, null, 0.5f, damage);

                EffectManager.Inst.CreateEffectObj(EffectObjType.Hit,
                    EnemyManager.Inst.enemys[0].hitPos.position + new Vector3(0, 0, -15), 0.2f, 1, damage);

                if (isTutorial)
                {
                    TalkWindow.Inst.SetFlagIndex(false);
                    TalkWindow.Inst.SetFlagNext(true);
                    TalkWindow.Inst.SetSkip(true);
                }
            }
        }

        if (MyHandCards.Count == 0)
        {
            TurnManager.Inst.EndTurn();
        }

        if (!isUse)
        {
            EnlargeCard(false, selectCard);
        }
    }

    private void UseCard(GameObject obj) //카드 사용
    {
        tombItemBuffer.Add(selectCard);
        MyHandCards.Remove(selectCard);
        CardAlignment();
        var position = selectCard.Parent.position;
        position = new Vector3(position.x, position.y, -3);
        selectCard.Parent.position = position;
        selectCard.originPRS.Pos = position;
        selectCard.originPRS.Scale = Vector3.one * 0.1f;
        selectCard.Use(obj);
        CardFinishMove();
    }

    private void CardDrag() //카드 드래그
    {
        if (!onMyCardArea)
        {
            selectCard.MoveTransform(
                new PRS(Utils.MousePos, Utils.CardRotate, selectCard.originPRS.Scale * 0.5f), false);
        }
        else
        {
            EnlargeCard(true, selectCard);
        }
    }

    public void CardFinishMove() //카드 사용 후 버린 카드 더미로 이동
    {
        selectCard.FinishCard();
        selectCard.GetComponent<Order>().SetOriginOrder(3700);
        waypoints = new Vector3[2];
        waypoints.SetValue(selectCard.Parent.position, 0);
        waypoints.SetValue(waypoint2.position, 0);
        waypoints.SetValue(cardEndPoint.position, 1);
        var target = selectCard.Parent.gameObject;
        target.transform.DOPath(waypoints, 1, PathType.CatmullRom).SetLookAt(cardEndPoint).SetEase(ease)
              .OnComplete(() => { target.SetActive(false); });
        selectCard = null;
    }

    private void EnlargeCard(bool isEnlarge, Card card, bool isUse = false) //카드 확대 함수
    {
        if (isEnlarge)
        {
            var enlargePos = new Vector3(card.originPRS.Pos.x, onMyCardArea ? -4 : card.originPRS.Pos.y,
                onMyCardArea ? -100 : card.originPRS.Pos.z);
            card.MoveTransform(new PRS(enlargePos, Utils.CardRotate, Vector3.one * (onMyCardArea ? 1.5f : 1)),
                false);
        }
        else
        {
            var parent = card.Parent.transform;
            card.MoveTransform(
                isUse
                    ? new PRS(parent.position, parent.rotation, Vector3.one)
                    : card.originPRS,
                false);
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    private void DetectCardArea() //마우스가 카드area에 있는지 체크
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    public void AddCardDeck(int card, int index = 1) //플레이어 카드 덱에 카드 추가, 상점이나 보상에서 카드 획득할 때
    {
        if (!IsCardDeckMax()[card])
        {
            cardDeck[card] += index;
        }
    }

    public bool[] IsCardDeckMax() //카드덱에 최대치가 된 카드가 있는지
    {
        var check = new bool[6];
        for (var i = 0; i < check.Length; i++)
        {
            check[i] = cardDeck[i] >= (18 - 3 * i);
        }

        return check;
    }

    public IEnumerator FixedCardNumToturial2Coroutine() //첫 전투 튜토리얼때 카드 숫자 고정
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

    private void LockMyHandCard(int index)
    {
        if (index < MyHandCards.Count)
        {
            MyHandCards[index].Lock();
        }
    }

    public void UnLockMyHandCard(int index)
    {
        if (index < MyHandCards.Count)
        {
            MyHandCards[index].UnLock();
        }
    }

    public void LockMyHandCardAll()
    {
        for (var i = 0; i < MyHandCards.Count; i++)
        {
            LockMyHandCard(i);
        }
    }

    public void UnLockMyHandCardAll()
    {
        for (var i = 0; i < MyHandCards.Count; i++)
        {
            UnLockMyHandCard(i);
        }
    }
}
