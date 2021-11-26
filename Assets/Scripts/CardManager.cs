using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public enum THROWING_OBJ_TYPE { CARDBACK, CARD_PIECE, NUM_CARD, QUESTION_CARD, SKILL_BOOK }

public class CardManager : MonoBehaviour
{
    public bool isIntro;
    public static CardManager Inst;
    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            if (!this.isIntro)
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

    public bool isTutorial;

    public int HandCardNumSum
    {
        get
        {
            int sum = 0;
            for (int i = 0; i < this.MyHandCards.Count; i++)
            {
                sum += this.MyHandCards[i].final_Num + 1;
            }
            return sum;
        }
    }

    //Quaternion cardRotate = Utils.QI;

    private void Start()
    {
        this.fixedCardNum = new int[12];
        for (int i = 0; i < this.fixedCardNum.Length; i++)
        {
            this.fixedCardNum[i] = -1;
        }
    }

    void Update()
    {
        if (MapManager.Inst.CurrentSceneName != "지도" && MapManager.Inst.CurrentSceneName != "상점")
        {
            if (this.isMyCardDrag) this.CardDrag();
            this.DetectCardArea();
        }
    }

    public Card PopItem()   //카드 뽑기
    {
        if (this.itemBuffer.Count == 0) this.CardTombToItemBuffer();

        Card card = this.itemBuffer[0];
        this.itemBuffer.RemoveAt(0);
        return card;
    }

    void SetupItemBuffer()  //초기 카드 생성
    {
        this.itemBuffer = new List<Card>();
        this.tombItemBuffer = new List<Card>();
        this.MyHandCards = new List<Card>();
        for (int i = 0; i < this.cardDeck.Length; i++)
        {
            for (int j = 0; j < this.cardDeck[i]; j++)
            {
                GameObject cardObj = Instantiate(this.cardPrefab, this.cardSpawnPoint.position, Utils.CardRotate);
                Card card = cardObj.GetComponentInChildren<Card>();
                cardObj.transform.localScale = Vector3.zero;
                cardObj.name = (i + 1).ToString();
                cardObj.gameObject.SetActive(false);
                card.Setup(i);
                this.itemBuffer.Add(card);
            }
        }
        for (int i = 0; i < this.itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, this.itemBuffer.Count);
            Card temp = this.itemBuffer[i];
            this.itemBuffer[i] = this.itemBuffer[rand];
            this.itemBuffer[rand] = temp;
        }
        for (int i = 0; i < this.fixedCardNum.Length; i++)
        {
            if (this.fixedCardNum[i] != -1)
            {
                this.itemBuffer[i].SetFinalNum(this.fixedCardNum[i]);
                this.fixedCardNum[i] = -1;
            }
        }
    }

    void ShuffleCard()      //버린 카드 더미를 섞는다
    {
        SoundManager.Inst.Play(CARDSOUND.Shuffling);
        for (int i = 0; i < this.tombItemBuffer.Count; i++)
        {
            int rand = Random.Range(i, this.tombItemBuffer.Count);
            Card temp = this.tombItemBuffer[i];
            this.tombItemBuffer[i] = this.tombItemBuffer[rand];
            this.tombItemBuffer[rand] = temp;
            this.tombItemBuffer[i].RevertOriginNum();
        }
    }

    public IEnumerator InitCoroutine()
    {
        this.cardSpawnPoint = GameObject.Find("CardSpawn").transform;
        this.cardEndPoint = GameObject.Find("CardEnd").transform;
        this.myCardLeft = GameObject.Find("CardLeft").transform;
        this.myCardRight = GameObject.Find("CardRight").transform;
        this.waypoint2 = GameObject.Find("WayPoint").transform;

        this.SetupItemBuffer();
        if (TurnManager.OnAddCard != null)
            TurnManager.OnAddCard -= this.AddCard;
        TurnManager.OnAddCard += this.AddCard;

        this.InitCard();
        yield return null;
    }

    void InitCard()         //카드 초기화
    {
        for (int i = 0; i < this.itemBuffer.Count; i++)
        {
            this.itemBuffer[i].parent.position = this.cardSpawnPoint.position;
            this.itemBuffer[i].parent.rotation = Utils.CardRotate;
            this.itemBuffer[i].parent.localScale = Vector3.zero;
        }
    }

    public void FinishBattle()          //전투가 끝날을때 호출
    {
        this.Init();
        TurnManager.OnAddCard -= this.AddCard;
        RewardManager.Inst.SetFinishBattleReward();
        this.StartCoroutine(TurnManager.Inst.ShowReward());
    }

    public void Init()
    {
        while (this.MyHandCards.Count > 0)
        {
            GameObject card = this.MyHandCards[0].parent.gameObject;
            this.MyHandCards.RemoveAt(0);
            Destroy(card);
        }
        while (this.itemBuffer.Count > 0)
        {
            GameObject card = this.itemBuffer[0].parent.gameObject;
            this.itemBuffer.RemoveAt(0);
            Destroy(card);
        }
        while (this.tombItemBuffer.Count > 0)
        {
            GameObject card = this.tombItemBuffer[0].parent.gameObject;
            this.tombItemBuffer.RemoveAt(0);
            Destroy(card);
        }

        this.selectCard = null;
        this.MyHandCards = null;
        this.itemBuffer = null;
        this.tombItemBuffer = null;
    }

    public void CardTombToItemBuffer()      //버린 카드 더미에서 뽑을 카드 더미로 섞고 이동
    {
        this.ShuffleCard();
        //StartCoroutine(CreateBeenCard());
        for (int i = 0; i < this.tombItemBuffer.Count; i++)
        {
            this.itemBuffer.Add(this.tombItemBuffer[i]);
        }
        for (int i = 0; i < this.tombItemBuffer.Count;)
        {
            this.tombItemBuffer.RemoveAt(0);
        }

        this.InitCard();
    }


    void OnDestroy()
    {
        TurnManager.OnAddCard -= this.AddCard;
    }


    public void TurnStartDraw()
    {
        this.StartCoroutine(this.DrawCardCoroutine(5));
    }

    public IEnumerator DrawCardCoroutine(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            this.AddCard();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SelectCardNumAdd(int index)
    {
        if (this.selectCard != null)
        {
            this.selectCard.AddNum(index);
        }
    }

    public void AddCard()           //카드 추가(카드 드로우시 사용)
    {
        SoundManager.Inst.Play(BATTLESOUND.CARD_DRAW);
        Card card = this.PopItem();
        card.parent.gameObject.SetActive(true);
        card.SetActiveChildObj(true);
        this.MyHandCards.Add(card);

        this.SetOriginOrder();
        this.CardAlignment();
    }

    void SetOriginOrder()           //카드 랜더링 순서 조정
    {
        int count = this.MyHandCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = this.MyHandCards[i];
            targetCard.GetComponent<Order>().SetOriginOrder(3700 + i * 10);
        }
    }

    void CardAlignment()            //카드 위치 조정
    {
        List<PRS> originCardPRSs = new List<PRS>();

        originCardPRSs = this.RoundAlignment(this.myCardLeft, this.myCardRight, this.MyHandCards.Count, 0.5f, Vector3.one);
        for (int i = 0; i < this.MyHandCards.Count; i++)
        {
            var targetCard = this.MyHandCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, this.CardMoveSpeed);
        }
    }

    public void FnishCardAllMyHand()
    {
        this.StartCoroutine(this.FinishTurnCoroutine());
    }

    public void FinishSceneAllMyHand()      //씬이 끌날때 손에 있는 모든 카드를 밑으로 내려버린다.
    {
        if (this.selectCard != null) this.isMyCardDrag = false;
        this.selectCard = null;
        for (int i = 0; i < this.MyHandCards.Count; i++)
        {
            this.MyHandCards[i].FinishScene();
        }
    }

    IEnumerator FinishTurnCoroutine()
    {
        for (int i = 0; i < this.MyHandCards.Count; i++)
        {
            this.MyHandCards[i].FinishCard();
        }
        for (int i = 0; i < this.MyHandCards.Count; i++)
        {
            yield return new WaitForEndOfFrame();
            PRS endPRS = new PRS(this.cardEndPoint.position, Utils.CardRotate, Vector3.one * 0.1f);
            var card = this.MyHandCards[i].GetComponent<Card>();
            card.transform.DOMove(this.cardEndPoint.position, 0.7f).OnComplete(() =>
            {
                this.MyHandCards.Remove(card);
                Destroy(card.gameObject);
            });
            this.tombItemBuffer.Add(card);
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
                objLerps = new[] { 0.5f };
                break;
            case 2:
                objLerps = new[] { 0.4f, 0.6f };
                break;
            case 3:
                objLerps = new[] { 0.3f, 0.5f, 0.7f };
                break;
            case 4:
                objLerps = new[] { 0.2f, 0.4f, 0.6f, 0.8f };
                break;
            case 5:
                objLerps = new[] { 0.1f, 0.3f, 0.5f, 0.7f, 0.9f };
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
        if (!this.isMyCardDrag) this.selectCard = card;
        if (this.selectCard == card && this.onMyCardArea)
        {
            this.EnlargeCard(true, card);
        }
    }

    public void CardMouseExit(Card card)
    {
        this.EnlargeCard(false, card);
    }

    public void CardMouseDown()
    {
        if (this.onMyCardArea)
        {
            SoundManager.Inst.Play(CARDSOUND.UP_CARD);
            this.isMyCardDrag = true;
        }
    }

    public void CardMouseUp()
    {
        if (this.isIntro)
        {
            this.isMyCardDrag = false;
            this.EnlargeCard(false, this.selectCard);
            switch (this.selectCard.final_Num)
            {
                case 0:
                    MapManager.Inst.LoadTutorialScene();
                    break;
                case 1: //옵션창
                    break;
                case 2:     //게임 종료
                    Application.Quit();
                    break;
            }
            return;
        }

        this.isMyCardDrag = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);

        int layer = LayerMask.NameToLayer("SkillBookCard");
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.layer == layer && SkillManager.Inst.ActivePage.isFinishFade)
            {
                hits[i].collider.GetComponent<SkillBookCard>().SetCard(this.selectCard);
                this.EnlargeCard(false, this.selectCard);
            }
        }

        layer = LayerMask.NameToLayer("Player");
        bool isUse = false;
        if (Array.Exists(hits, x => x.collider.gameObject.layer == layer) && hits.Length <= 2)      //만약 플레이어라면
        {
            this.EnlargeCard(false, this.selectCard, true);
            isUse = true;
            this.UseCard(Player.Inst.gameObject);

            if (this.isTutorial)
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
                this.EnlargeCard(false, this.selectCard, true);

                isUse = true;

                int damage = this.selectCard.final_Num == EnemyManager.Inst.enemys[0].GetComponent<Enemy>().weaknessNum ? this.selectCard.final_Num + 1 : 1;

                this.UseCard(EnemyManager.Inst.enemys[0].gameObject);

                ThrowingObjManager.Inst.CreateThrowingObj(THROWING_OBJ_TYPE.CARDBACK,
                    Player.Inst.gameObject.transform.position + Vector3.up * 3.5f, EnemyManager.Inst.enemys[0].hitPos.position, null, 0.5f, damage);

                EffectManager.Inst.CreateEffectObj(EffectObjType.HIT, EnemyManager.Inst.enemys[0].hitPos.position + new Vector3(0, 0, -15), 0.2f, 1, damage);

                if (this.isTutorial)
                {
                    TalkWindow.Inst.SetFlagIndex(false);
                    TalkWindow.Inst.SetFlagNext(true);
                    TalkWindow.Inst.SetSkip(true);
                }

            }
        }

        if (this.MyHandCards.Count == 0)
            TurnManager.Inst.EndTurn();

        if (!isUse) this.EnlargeCard(false, this.selectCard);
    }

    void UseCard(GameObject obj)        //카드 사용
    {
        this.tombItemBuffer.Add(this.selectCard);
        this.MyHandCards.Remove(this.selectCard);
        this.CardAlignment();
        this.selectCard.parent.position = new Vector3(this.selectCard.parent.position.x, this.selectCard.parent.position.y, -3);
        this.selectCard.originPRS.pos = this.selectCard.parent.position;
        this.selectCard.originPRS.scale = Vector3.one * 0.1f;
        this.selectCard.Use(obj);
        this.CardFinishMove();
    }

    IEnumerator CreateBeenCard()
    {
        for (int i = 0; i < this.tombItemBuffer.Count; i++)
        {
            var cardObject = Instantiate(this.beenCardPrefab, this.cardEndPoint.position, Utils.CardRotate);
            cardObject.transform.DOMove(this.cardSpawnPoint.position, 1f).OnComplete(() =>
            {
                Destroy(cardObject);
            });
            yield return new WaitForEndOfFrame();
        }
    }

    void CardDrag()             //카드 드래그
    {
        if (!this.onMyCardArea)
        {
            this.selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.CardRotate, this.selectCard.originPRS.scale * 0.5f), false);
        }
        else
        {
            this.EnlargeCard(true, this.selectCard);
        }
    }

    public void CardFinishMove()            //카드 사용 후 버린 카드 더미로 이동
    {
        this.selectCard.FinishCard();
        this.selectCard.GetComponent<Order>().SetOriginOrder(3700);
        this.waypoints = new Vector3[2];
        this.waypoints.SetValue(this.selectCard.parent.position, 0);
        this.waypoints.SetValue(this.waypoint2.position, 0);
        this.waypoints.SetValue(this.cardEndPoint.position, 1);
        GameObject target = this.selectCard.parent.gameObject;
        target.transform.DOPath(this.waypoints, 1, PathType.CatmullRom).
            SetLookAt(this.cardEndPoint).SetEase(this.ease).OnComplete(() =>
            {
                target.SetActive(false);
            });
        this.selectCard = null;
    }

    public IEnumerator MoveCenterCardCoroutine(Card card)
    {
        card.parent.DORotateQuaternion(Utils.CardRotate, 0.5f);
        card.parent.DOMove(this.CardCenterPoint.position, 0.5f).OnComplete(() =>
        {
            this.CardFinishMove();
        });
        yield return null;
    }

    void EnlargeCard(bool isEnlarge, Card card, bool isUse = false)         //카드 확대 함수
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, this.onMyCardArea ? -4 : card.originPRS.pos.y, this.onMyCardArea ? -100 : card.originPRS.pos.z);
            card.MoveTransform(new PRS(enlargePos, Utils.CardRotate, Vector3.one * (this.onMyCardArea ? 1.5f : 1)), false);
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
        this.onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    public void AddCardDeck(Card card)      //플레이어 카드 덱에 카드 추가, 상점이나 보상에서 카드 획득할 때
    {
        this.cardDeck[card.original_Num]++;
    }

    public void AddCardDeck(int card, int index = 1)      //플레이어 카드 덱에 카드 추가, 상점이나 보상에서 카드 획득할 때
    {
        if (!this.isCardDeckMax()[card]) this.cardDeck[card] += index;
    }

    public bool[] isCardDeckMax()       //카드덱에 최대치가 된 카드가 있는지
    {
        bool[] check = new bool[6];
        for (int i = 0; i < check.Length; i++)
        {
            check[i] = this.cardDeck[i] >= (18 - 3 * i);
        }
        return check;
    }

    public IEnumerator StartIntroCard()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            Card card = Instantiate(this.IntroCardPrefab, this.cardSpawnPoint.position, Utils.CardRotate).GetComponentInChildren<Card>();
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
            }

            this.MyHandCards.Add(card.GetComponentInChildren<Card>());
            this.SetOriginOrder();
            this.CardAlignment();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public IEnumerator FixedCardNumToturial2Coroutine()          //첫 전투 튜토리얼때 카드 숫자 고정
    {
        this.fixedCardNum[0] = 2;
        this.fixedCardNum[1] = 5;
        this.fixedCardNum[2] = 3;

        this.fixedCardNum[3] = 0;
        this.fixedCardNum[4] = 1;
        this.fixedCardNum[5] = 4;

        this.fixedCardNum[6] = 4;
        this.fixedCardNum[7] = 3;
        this.fixedCardNum[8] = 2;

        this.fixedCardNum[9] = 2;
        this.fixedCardNum[10] = 1;
        this.fixedCardNum[11] = 3;
        yield return null;
    }

    public void LockMyHandCard(int index)
    {
        if (index < this.MyHandCards.Count) this.MyHandCards[index].Lock();
    }

    public void UnLockMyHandCard(int index)
    {
        if (index < this.MyHandCards.Count) this.MyHandCards[index].UnLock();
    }

    public void LockMyHandCardAll()
    {
        for (int i = 0; i < this.MyHandCards.Count; i++)
        {
            this.LockMyHandCard(i);
        }
    }

    public void UnLockMyHandCardAll()
    {
        for (int i = 0; i < this.MyHandCards.Count; i++)
        {
            this.UnLockMyHandCard(i);
        }
    }
}
