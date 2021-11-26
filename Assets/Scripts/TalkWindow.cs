using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TalkWindow : MonoBehaviour
{
    public static TalkWindow Inst;

    [SerializeField] private Ghost ghost;
    [SerializeField] private TMP_Text talkTMP;
    public List<List<string>> talks;
    [SerializeField] private bool isSkip;
    [SerializeField] private bool isFlagNext;
    [SerializeField] private bool isFlagIndex;    //대화가 계속 진행되지 않도록 거는 플래그
    public int index;
    [SerializeField] private int _index2;
    public int index2
    {
        get
        {
            return _index2;
        }
        set
        {
            _index2 = Mathf.Clamp(value, 0, talks[index].Count - 1);
        }
    }
    public string currentTalk
    {
        get
        {
            return talks[Mathf.Clamp(index, 0, talks.Count - 1)][Mathf.Clamp(index2, 0, talks[index].Count - 1)];
        }
    }

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }

        //Time.timeScale = 2;

        talks = new List<List<string>>();
        for (int i = 0; i < 16; i++)
        {
            talks.Add(new List<string>());
        }

        talks[0].Add("일어났군! 기억이 없어서 아주 \n혼란스러울 거야, 난 이 미궁에서 목숨을 잃은 모험가지.");
        talks[0].Add("난 미궁을 탈출하기 위해 보스한테 도전했지만 결국 실패했어.");
        talks[0].Add("자네가 나의 복수를 해주게. \n그렇다면 보스에게 도달하는 \n곳까지 도움을 주지.");
        talks[0].Add("주변에 있는 무덤이 보일 거야. \n무덤에 있는 카드, 가방, 스킬북 \n전부 챙겨보자고");
        talks[0].Add("전부 챙겼으면 어떻게 싸우는지 \n알려줄 테니 일단 다음 필드로 \n넘어 가보자");

        talks[1].Add("현재 보이는 화면이 맵이라네. \n만약에 더 앞을 보고 싶으면 \n빈 공간을 클릭해서 드래그하면 된다네.");
        talks[1].Add("여기서 아이콘을 클릭하면 해당 \n필드로 갈 수 있지.");
        talks[1].Add("우리가 다녀간 필드는 초록색 체크 표시가 생기며 다시 돌아갈 수 \n없다네.");
        talks[1].Add("앞에 둥그렇게 생긴 필드가 \n보일 거야. 몬스터와 전투하여 \n보상을 얻는 전투 필드라네.");

        talks[2].Add("앞에 몬스터 머리 위에 있는 \n3이라는 숫자가 보이나? \n그 숫자가 적의 약점 숫자라네.");
        talks[2].Add("손에 들고 있는 3 카드로 한번 공격해보게. 그 숫자 그대로 데미지가 들어갈 거야.");
        talks[2].Add("이번엔 6 카드로 한번 공격해봐. 숫자가 높아도 약점 숫자랑 \n다르다면 데미지가 1밖에 \n안 들어갈 거네.");
        talks[2].Add("약점 숫자 뒤에 아이콘은 적의 \n패턴이라네. 검일 땐 공격, \n십자가일 땐 회복이지.");
        talks[2].Add("나머지 카드를 자네에게 써보게. 그렇다면 해당 숫자만큼의 실드가 생길 거라네.");
        talks[2].Add("실드가 4만큼 생겼을 거라네. \n실드는 적의 공격을 실드 숫자만큼 방어해주지.");
        talks[2].Add("그리고 손에 들고 있는 카드를 다 \n사용하면 몬스터의 턴으로 넘어가게 된다네. 계속 싸워서 \n이겨보게나.");

        talks[3].Add("이번엔 가방을 알려주지. 화살표가 가르키는 가방 아이콘을 눌러보게나.");
        talks[3].Add("왼쪽에는 각 카드마다 보유 개수와 최대 보유 개수가 있고 오른쪽에는 보유 스킬을 확인할 수 있지.");
        talks[3].Add("가방을 끄고 싶다면 가방 오른쪽 \n상단 닫기 버튼을 한 번 더 \n누르거나 단축키 I를 사용하면 \n된다네.");

        talks[4].Add("이번 필드에서는 스킬북에 대해서 알려주지. 오른쪽 상단에 있는 책을 눌러보게.");
        talks[4].Add("화살표가 가리키는 버튼을 통해서 다른 스킬로 변경 할 수 있다네.");
        talks[4].Add("화살표가 가리키는 곳에 해당 \n스킬에 대한 설명이 있다네. 스킬마다 다르니 한번 확인해보도록.");
        talks[4].Add("만약 스킬을 사용하고 싶다면 \n화살표가 가리키는 곳에 카드를 \n드래그해서 올려두게.");
        talks[4].Add("그렇다면 오른쪽 페이지의 \n버튼으로 카드의 숫자를 변경시킬 수 있을 거라네.");
        talks[4].Add("스킬들은 필드마다 한 번씩 쓸 수 있으니 신중하게 쓰도록 하게나. 다음 필드에 가면 다시 쓸 수 있을 거라네.");
        talks[4].Add("스킬창을 끄고 싶다면 스킬창 \n오른쪽 상단 닫기 버튼을 한 번 더 누르거나 단축키 K를 사용하면 \n된다네.");

        talks[5].Add("지금부터 전투 필드를 고르게 되면 저주를 선택해야 될 거야.");
        talks[5].Add("저주는 해당 전투 필드 동안 \n적용되는 것으로 유리한걸 \n선택해야 하네.");
        talks[5].Add("저주는 랜덤 된 3가지가 나오는데 그 중 하나를 선택하면 돼.");

        talks[6].Add("선택한 저주는 오른쪽 상단 \n저주창에서 확인할 수 있다네.");
        talks[6].Add("저주창 왼쪽 버튼을 클릭하면 \n저주창을 숨기거나 보이게 할 수 \n있다네.");

        talks[7].Add("앞에 필드가 다르게 생긴 게 \n보일 거야. 너의 체력을 회복시켜줄 수 있는 휴식 필드라네.");

        talks[8].Add("가운데 버튼을 누르면 너의 체력이 20만큼 회복이 될 거야.");

        talks[9].Add("앞에 필드가 다르게 생긴 게 \n보일 거야. 다양한 이벤트가 있는 필드라네.");

        talks[10].Add("각각의 선택지들은 손에 들고 있는 카드 숫자의 합이라는 조건을 \n가지고 있지.");
        talks[10].Add("이벤트 필드도 스킬을 사용할 수 \n있으니 참고하게나.");
        talks[10].Add("테두리가 초록색인 선택지를 선택 할 수 있을 거라네.");

        talks[11].Add("왼쪽에 다르게 생긴 필드가 \n있을 거야. 이 필드는 숫자 카드와 시작 드로우 수를 늘릴 수 있는 상점 필드라네.");

        talks[12].Add("카드를 클릭하면 구매할 수 있고 \n해당 카드의 가격은 카드 밑에 \n표시되어 있다네.");
        talks[12].Add("만약 물음표 카드가 있다면 숫자 카드를 구매할 때 먼저 \n소비된다네.");
        talks[12].Add("드로우 수 증가는 최대 6장이니 \n총 3번 구매할 수 있다네.");
        talks[12].Add("최대치까지 구매했으면 더는 \n구매를 못할 거라네.");
        talks[12].Add("이 상점 필드는 이후에도 다시 \n방문이 가능하니 참고하고 오른쪽 하단에 지도 아이콘을 클릭해서 \n나갈 수 있다네.");

        talks[13].Add("드디어 우리가 보스 방까지 \n도달했군. 부디 나의 복수를 이루어 주게나.");
        talks[13].Add("보스는 생각보다 강력한 공격을 가지고 있다네. 조심하게나.");

        talks[14].Add("나의 복수를 해줘서 고맙구나. \n자네도 그 동안 수고했네. \n그럼 난 이만 가보겠네.");

        talks[15].Add(" ");
    }

    public void ShowTalk()
    {
        StartCoroutine(ShowText(index));
    }

    public IEnumerator ShowText(int index)
    {
        talkTMP.text = "";
        yield return StartCoroutine(ShowTalkWindowCoroutine());
        //StartCoroutine(ShowTalkCoroutine(index));
    }

    public IEnumerator ShowTalkWindowCoroutine()
    {
        talkTMP.text = "";
        //Tween tween = this.GetComponent<SpriteRenderer>().DOFade(1, 1);
        Tween tween = GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        yield return tween.WaitForCompletion();
    }

    private IEnumerator ShowTalkCoroutine(int index)
    {
        for (int i = 0; i < talks[index].Count; i++)
        {
            yield return StartCoroutine(TalkTypingCoroutine(index, i));
            yield return StartCoroutine(CheckFlagIndexCoroutine());
            yield return StartCoroutine(CheckFlagNextCoroutine());
        }

        StartCoroutine(HideText());
    }

    public IEnumerator TalkTypingCoroutine(int index, int index2)       //한 문장 텍스트 타이핑 효과 코루틴
    {
        isSkip = false;
        this.index = index;
        this.index2 = index2;
        for (int i = 0; i < talks[index][index2].Length; i++)
        {
            if (isSkip)
            {
                isSkip = false;
                talkTMP.text = currentTalk;
                break;
            }

            talkTMP.text = talks[index][index2].Substring(0, i + 1);
            yield return new WaitForSeconds(0.03f);
        }
        yield return null;
    }

    public IEnumerator CheckFlagIndexCoroutine()
    {
        while (true)
        {
            if (!isFlagIndex)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public IEnumerator CheckFlagNextCoroutine()
    {
        while (true)
        {
            if (isFlagNext)
            {
                isFlagNext = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public IEnumerator HideText()
    {
        talkTMP.text = "";
        //Tween tween = this.GetComponent<SpriteRenderer>().DOFade(0, 1);
        Tween tween = GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        yield return tween.WaitForCompletion();
        index++;
        index2 = 0;
        yield return StartCoroutine(ghost.HideTalk());
    }

    public void SetFlagIndex(bool flag)
    {
        isFlagIndex = flag;
    }

    public void SetFlagNext(bool flag)
    {
        isFlagNext = flag;
    }

    public void SetSkip(bool flag)
    {
        isSkip = flag;
    }

    public void InitFlag()
    {
        SetFlagIndex(false);
        SetFlagNext(false);
        SetSkip(false);
    }

    private void OnMouseUp()
    {
        SoundManager.Inst.Play(EVENTSOUND.CHOICE_BUTTON);
        if (talkTMP.text.Length == currentTalk.Length)
        {
            SetFlagNext(true);
            index2++;
        }
        else
        {
            isSkip = true;
        }
    }
}
