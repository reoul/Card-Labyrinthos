﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }

    //[SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject beenCardPrefab;
    [SerializeField] Transform cardSpawnPoint;  //뽑기 카드 더미 위치
    [SerializeField] Transform cardEndPoint;  //버린 카드 더미 위치
    [SerializeField] Transform myCardLeft;  //내 손패 왼쪽 포지션
    [SerializeField] Transform myCardRight; //내 손패 오른쪽 포지션
    [SerializeField] Transform CardCenterPoint;

    [SerializeField] int[] cardDeck;        //현재 플레이어 카드 덱, 1~6
    [SerializeField] List<Card> MyHandCards;    //내 손에 들고 있는 카드 리스트
    [SerializeField] List<Card> itemBuffer;  //뽑을 카드 더미
    [SerializeField] List<Card> tombItemBuffer;  //버린 카드 더미, 사용한 카드가 여기 리스트에 쌓인다

    public Card selectCard;     //선택된 카드

    bool isMyCardDrag;          //현재 플레이어가 카드를 드래그 중인지
    bool onMyCardArea;          //플레이어 마우스가 카드Area에 있는지

    public Ease ease;

    public Transform waypoint2;

    private Vector3[] waypoints;        //카드 사용후 버린 카드더미로 이동할때 사용

    //Quaternion cardRotate = Utils.QI;

    void Start()
    {
        cardDeck = new int[6];
        for (int i = 0; i < cardDeck.Length; i++)
            cardDeck[i] = 1;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Battle")
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
    }

    void ShuffleCard()      //버린 카드 더미를 섞는다
    {
        for (int i = 0; i < tombItemBuffer.Count; i++)
        {
            int rand = Random.Range(i, tombItemBuffer.Count);
            Card temp = tombItemBuffer[i];
            tombItemBuffer[i] = tombItemBuffer[rand];
            tombItemBuffer[rand] = temp;
        }
    }

    public IEnumerator InitCorutine()
    {
        cardSpawnPoint = GameObject.Find("CardSpawn").transform;
        cardEndPoint = GameObject.Find("CardEnd").transform;
        myCardLeft = GameObject.Find("CardLeft").transform;
        myCardRight = GameObject.Find("CardRight").transform;
        waypoint2 = GameObject.Find("WayPoint").transform;

        SetupItemBuffer();
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

    void FinishBattle()
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
        TurnManager.OnAddCard -= AddCard;
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


    void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
    }


    public void TurnStartDraw()
    {
        StartCoroutine(DrawCardCorutine(5));
    }

    public IEnumerator DrawCardCorutine(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            AddCard();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddCard()           //카드 추가(카드 드로우시 사용)
    {
        Card card = PopItem();
        card.parent.gameObject.SetActive(true);
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
            targetCard.GetComponent<Order>().SetOriginOrder(i);
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
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    public void FnishCardAllMyHand()
    {
        StartCoroutine(FinishTurnCorutine());
    }

    IEnumerator FinishTurnCorutine()
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
            EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        EnlargeCard(false, card);
    }

    public void CardMouseDown()
    {
        if (onMyCardArea)
            isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("Player");
        bool isUse = false;
        if (Array.Exists(hits, x => x.collider.gameObject.layer == layer))      //만약 플레이어라면
        {
            EnlargeCard(false, selectCard, true);
            isUse = true;
            UseCard(Player.Inst.gameObject);
            if (MyHandCards.Count == 0)
                TurnManager.Inst.EndTurn();
        }
        else
        {
            layer = LayerMask.NameToLayer("Enemy");
            if (Array.Exists(hits, x => x.collider.gameObject.layer == layer))      //만약 적이라면
            {
                EnlargeCard(false, selectCard, true);
                isUse = true;
                UseCard(EnemyManager.Inst.enemys[0].gameObject);
                if (MyHandCards.Count == 0)
                    TurnManager.Inst.EndTurn();
            }
        }
        if (!isUse)
            EnlargeCard(false, selectCard);
    }

    void UseCard(GameObject obj)        //카드 사용
    {
        tombItemBuffer.Add(selectCard);
        MyHandCards.Remove(selectCard);
        CardAlignment();
        selectCard.parent.position = new Vector3(selectCard.parent.position.x, selectCard.parent.position.y, 0);
        selectCard.originPRS.pos = selectCard.parent.position;
        selectCard.originPRS.scale = Vector3.one * 0.1f;
        selectCard.Use(obj);
        CardFinishMove();
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
        waypoints = new Vector3[3];
        waypoints.SetValue(selectCard.parent.position, 0);
        waypoints.SetValue(waypoint2.position, 1);
        waypoints.SetValue(cardEndPoint.position, 2);
        GameObject target = selectCard.parent.gameObject;
        selectCard.parent.DOPath(waypoints, 1, PathType.CatmullRom).
            SetLookAt(new Vector3(0, 0, 0)).SetEase(ease).OnComplete(() =>
            {
                target.SetActive(false);
            });
        selectCard = null;
    }

    public IEnumerator MoveCenterCardCorutine(Card card)
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
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, onMyCardArea ? -3 : card.originPRS.pos.y, onMyCardArea ? -100 : card.originPRS.pos.z);
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

    public void AddCardDeck(int card)      //플레이어 카드 덱에 카드 추가, 상점이나 보상에서 카드 획득할 때
    {
        cardDeck[card]++;
    }

}