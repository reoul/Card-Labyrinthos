using System;
using UnityEngine;

public enum BACKGROUNDSOUND { INTRO, TUTORIAL, MAP, BATTLE, EVENT, SHOP, REST, BOSS, ENDING }
public enum SKILLBOOKSOUND { OPEN_BOOK, CLOSE_BOOK, CARD_ON_BOOK, CARD_NUM_UP_DOWN, TURN_PAGE }
public enum MAPSOUND { CHOICE_FIELD, OPEN_DEBUFFWINDOW, CHOICE_DEBUFF, SHOW_DEBUFF_BUTTON }
public enum BATTLESOUND { HEAL, CARD_DRAW, SHELD, HIT, TURN_START, TURN_END, GAME_WIN, GAME_FAILD }
public enum EVENTSOUND { CHOICE_MOUSEUP, CHOICE_BUTTON }
public enum SHOPSOUND { BUY, SOLDOUT, THROWINGOBJ, IN_TOPBAR_ICON }
public enum REWARDSOUND { SHOW_REWARD_WINDOW, GETQUESTION, GETCARDPIECE, LOSTHEAL, SHOW_REWARD_BUTTON }
public enum CARDSOUND { UP_CARD, GO_BACK, Shuffling }
public enum RESTSOUND { HEAL }
public enum DEBUFFSOUND { OPEN_BAR, CLOSE_BAR }


[Serializable]
public class Sounds
{
    [Header("스킬북")]
    public AudioClip skillbook_openBook;
    public AudioClip skillbook_closeBook;
    [Header("카드 스킬북에 올려놓는 소리")]
    public AudioClip skillbook_cardOnBook;
    public AudioClip skillbook_cardNumUpDown;
    public AudioClip skillbook_turnPage;
    [Header("지도")]
    public AudioClip map_choiceField;
    public AudioClip map_openDebuffWindow;
    public AudioClip map_choiceDebuff;
    public AudioClip map_showDebuffButton;
    [Header("전투")]
    public AudioClip battle_heal;
    public AudioClip battle_cardDraw;
    public AudioClip battle_sheld;
    public AudioClip battle_hit;
    public AudioClip battle_turnStart;
    public AudioClip battle_turnEnd;
    public AudioClip battle_gameWin;
    public AudioClip battle_gameFaild;
    [Header("이벤트")]
    public AudioClip event_choiceMouseUp;
    public AudioClip event_choiceButton;
    [Header("상점")]
    public AudioClip shop_buyitem;
    public AudioClip shop_soldOut;
    public AudioClip shop_throwingObj;
    public AudioClip shop_inTopBarIcon;
    [Header("보상창")]
    public AudioClip reward_showRewardWindow;
    public AudioClip reward_getQuestion;
    public AudioClip reward_getCardPiece;
    public AudioClip reward_lostHeal;
    public AudioClip reward_showRewardButton;
    [Header("카드")]
    public AudioClip card_upCard;
    public AudioClip card_goback;
    public AudioClip card_shuffling;
    [Header("휴식방")]
    public AudioClip rest_heal;
    [Header("휴식방")]
    public AudioClip debuff_openbar;
    public AudioClip debuff_closebar;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;

    [SerializeField] private Sounds sounds;

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

    public AudioClip[] BackGroundAudio;
    public AudioClip[] SFXAudio;

    public AudioSource BackGroundAudioSource;
    public SFXSound[] SFXAudioSources;

    public void SetBGMVolume(float volume)
    {
        BackGroundAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        for (int i = 0; i < SFXAudioSources.Length; i++)
        {
            SFXAudioSources[i].SetVolume(volume);
        }
    }

    public void Play(BACKGROUNDSOUND sound)
    {
        BackGroundAudioSource.clip = GetAudio(sound);
        BackGroundAudioSource.Play();
    }
    public void Play(SKILLBOOKSOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }
    public void Play(MAPSOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }
    public void Play(BATTLESOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }
    public void Play(EVENTSOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }
    public void Play(SHOPSOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }
    public void Play(REWARDSOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }
    public void Play(CARDSOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }

    public void Play(RESTSOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }

    public void Play(DEBUFFSOUND sound)
    {
        SFXPlay(GetAudio(sound));
    }
    public void SFXPlay(AudioClip clip)
    {
        for (int i = 0; i < SFXAudioSources.Length; i++)
        {
            if (!SFXAudioSources[i].isPlaying)
            {
                SFXAudioSources[i].Play(clip);
                break;
            }
        }
    }

    public AudioClip GetAudio(BACKGROUNDSOUND sound)
    {
        return BackGroundAudio[(int)sound];
    }
    public AudioClip GetAudio(SKILLBOOKSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case SKILLBOOKSOUND.OPEN_BOOK:
                clip = sounds.skillbook_openBook;
                break;
            case SKILLBOOKSOUND.CLOSE_BOOK:
                clip = sounds.skillbook_closeBook;
                break;
            case SKILLBOOKSOUND.CARD_ON_BOOK:
                clip = sounds.skillbook_cardOnBook;
                break;
            case SKILLBOOKSOUND.CARD_NUM_UP_DOWN:
                clip = sounds.skillbook_cardNumUpDown;
                break;
            case SKILLBOOKSOUND.TURN_PAGE:
                clip = sounds.skillbook_turnPage;
                break;
            default:
                clip = sounds.skillbook_openBook;
                break;
        }
        return clip;
    }
    public AudioClip GetAudio(MAPSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case MAPSOUND.CHOICE_FIELD:
                clip = sounds.map_choiceField;
                break;
            case MAPSOUND.OPEN_DEBUFFWINDOW:
                clip = sounds.map_openDebuffWindow;
                break;
            case MAPSOUND.CHOICE_DEBUFF:
                clip = sounds.map_choiceDebuff;
                break;
            case MAPSOUND.SHOW_DEBUFF_BUTTON:
                clip = sounds.map_showDebuffButton;
                break;
            default:
                clip = sounds.map_choiceField;
                break;
        }
        return clip;
    }
    public AudioClip GetAudio(BATTLESOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case BATTLESOUND.HEAL:
                clip = sounds.battle_heal;
                break;
            case BATTLESOUND.CARD_DRAW:
                clip = sounds.battle_cardDraw;
                break;
            case BATTLESOUND.SHELD:
                clip = sounds.battle_sheld;
                break;
            case BATTLESOUND.HIT:
                clip = sounds.battle_hit;
                break;
            case BATTLESOUND.TURN_START:
                clip = sounds.battle_turnStart;
                break;
            case BATTLESOUND.TURN_END:
                clip = sounds.battle_turnEnd;
                break;
            case BATTLESOUND.GAME_WIN:
                clip = sounds.battle_gameWin;
                break;
            case BATTLESOUND.GAME_FAILD:
                clip = sounds.battle_gameFaild;
                break;
            default:
                clip = sounds.battle_heal;
                break;
        }
        return clip;
    }
    public AudioClip GetAudio(EVENTSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case EVENTSOUND.CHOICE_MOUSEUP:
                clip = sounds.event_choiceMouseUp;
                break;
            case EVENTSOUND.CHOICE_BUTTON:
                clip = sounds.event_choiceButton;
                break;
            default:
                clip = sounds.event_choiceButton;
                break;
        }
        return clip;
    }
    public AudioClip GetAudio(SHOPSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case SHOPSOUND.BUY:
                clip = sounds.shop_buyitem;
                break;
            case SHOPSOUND.SOLDOUT:
                clip = sounds.shop_soldOut;
                break;
            case SHOPSOUND.THROWINGOBJ:
                clip = sounds.shop_throwingObj;
                break;
            case SHOPSOUND.IN_TOPBAR_ICON:
                clip = sounds.shop_inTopBarIcon;
                break;
            default:
                clip = sounds.shop_buyitem;
                break;
        }
        return clip;
    }
    public AudioClip GetAudio(REWARDSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case REWARDSOUND.SHOW_REWARD_WINDOW:
                clip = sounds.reward_showRewardWindow;
                break;
            case REWARDSOUND.GETQUESTION:
                clip = sounds.reward_getQuestion;
                break;
            case REWARDSOUND.GETCARDPIECE:
                clip = sounds.reward_getCardPiece;
                break;
            case REWARDSOUND.LOSTHEAL:
                clip = sounds.reward_lostHeal;
                break;
            case REWARDSOUND.SHOW_REWARD_BUTTON:
                clip = sounds.reward_showRewardButton;
                break;
            default:
                clip = sounds.reward_getQuestion;
                break;
        }
        return clip;
    }
    public AudioClip GetAudio(CARDSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case CARDSOUND.UP_CARD:
                clip = sounds.card_upCard;
                break;
            case CARDSOUND.GO_BACK:
                clip = sounds.card_goback;
                break;
            case CARDSOUND.Shuffling:
                clip = sounds.card_shuffling;
                break;
            default:
                clip = sounds.card_upCard;
                break;
        }
        return clip;
    }
    public AudioClip GetAudio(RESTSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case RESTSOUND.HEAL:
                clip = sounds.rest_heal;
                break;
            default:
                clip = sounds.rest_heal;
                break;
        }
        return clip;
    }

    public AudioClip GetAudio(DEBUFFSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case DEBUFFSOUND.OPEN_BAR:
                clip = sounds.debuff_openbar;
                break;
            case DEBUFFSOUND.CLOSE_BAR:
                clip = sounds.debuff_closebar;
                break;
            default:
                clip = sounds.debuff_openbar;
                break;
        }
        return clip;
    }
}
