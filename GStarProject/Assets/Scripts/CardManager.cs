﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;


public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake()
    {
        Inst = this;
    }

    //[SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject beenCardPrefab;
    [SerializeField] Transform cardSpawnPoint;  //뽑기 카드 더미 위치
    [SerializeField] Transform cardEndPoint;  //버린 카드 더미 위치
    [SerializeField] Transform myCardLeft;  //내 손패 왼쪽 포지션
    [SerializeField] Transform myCardRight; //내 손패 오른쪽 포지션
    [SerializeField] Transform CardCenterPoint;

    int[] cardCount;        //현재 가지고 있는 1~6카드 갯수
    [SerializeField] List<Card> MyHandCards;    //내 손에 들고 있는 카드 리스트
    [SerializeField] List<Card> itemBuffer;  //뽑을 카드 더미
    [SerializeField] List<Card> tombItemBuffer;  //버린 카드 더미, 사용한 카드가 여기 리스트에 쌓인다

    public Card selectCard;

    bool isMyCardDrag;
    bool onMyCardArea;

    public Ease ease;

    public Transform waypoint2;

    private Vector3[] waypoints;

    //Quaternion cardRotate = Utils.QI;

    void Start()
    {
        cardCount = new int[6];
        for (int i = 0; i < cardCount.Length; i++)
            cardCount[i] = 1;
        SetupItemBuffer();
        TurnManager.OnAddCard += AddCard;
        //Time.timeScale = 0.1f;
    }

    void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        DetectCardArea();
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
        for (int i = 0; i < cardCount.Length; i++)
        {
            for (int j = 0; j < cardCount[i]; j++)
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

    void InitCard()
    {
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            itemBuffer[i].parent.position = cardSpawnPoint.position;
            itemBuffer[i].parent.rotation = Utils.CardRotate;
            itemBuffer[i].parent.localScale = Vector3.zero;
        }
    }

    public void CardTombToItemBuffer()
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
            cardObject.transform.DOMove(cardSpawnPoint.position,1f).OnComplete(()=>
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

    public void AddCard()
    {
        Card card = PopItem();
        card.parent.gameObject.SetActive(true);
        MyHandCards.Add(card);

        SetOriginOrder();
        CardAlignment();
    }

    void SetOriginOrder()
    {
        int count = MyHandCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = MyHandCards[i];
            targetCard.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void CardAlignment()
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
            card.transform.DOMove(cardEndPoint.position, 0.7f).OnComplete(()=>
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

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
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
        selectCard = card;
        EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        EnlargeCard(false, card);
    }

    public void CardMouseDown()
    {
        isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("Player");
        if (Array.Exists(hits, x => x.collider.gameObject.layer == layer))
        {
            UseCard(Player.Inst.gameObject);
            if (MyHandCards.Count == 0)
                TurnManager.Inst.EndTurn();
        }
        else
        {
            layer = LayerMask.NameToLayer("Enemy");
            if (Array.Exists(hits, x => x.collider.gameObject.layer == layer))
            {
                UseCard(EnemyManager.Inst.enemys[0].gameObject);
                if (MyHandCards.Count == 0)
                    TurnManager.Inst.EndTurn();
            }
        }
    }

    void UseCard(GameObject obj)
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

    void CardDrag()
    {
        if(!onMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.CardRotate, selectCard.originPRS.scale * 0.5f), false);
        }
        else
        {
            EnlargeCard(true, selectCard);
        }
    }

    public void CardFinishMove()
    {
        selectCard.FinishCard();
        waypoints = new Vector3[3];
        waypoints.SetValue(selectCard.parent.position, 0);
        waypoints.SetValue(waypoint2.position, 1);
        waypoints.SetValue(cardEndPoint.position, 2);
        GameObject target = selectCard.parent.gameObject;
        selectCard.parent.DOPath(waypoints, 1, PathType.CatmullRom).
            SetLookAt(new Vector3(0, 0, 0)).SetEase(ease).OnComplete(()=>
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

    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        { 
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, onMyCardArea ? -3 : card.originPRS.pos.y, onMyCardArea ? -100 : card.originPRS.pos.z);
            card.MoveTransform(new PRS(enlargePos, Utils.CardRotate, Vector3.one * (onMyCardArea ? 1.5f : 1)), false);
        }
        else
        {
            card.MoveTransform(card.originPRS, false);
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }
}
