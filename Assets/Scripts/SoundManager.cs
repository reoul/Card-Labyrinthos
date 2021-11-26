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

    [SerializeField] Sounds sounds;

    private void Awake()
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

    public AudioClip[] BackGroundAudio;
    public AudioClip[] SFXAudio;

    public AudioSource BackGroundAudioSource;
    public SFXSound[] SFXAudioSources;

    public void SetBGMVolume(float volume)
    {
        this.BackGroundAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        for (int i = 0; i < this.SFXAudioSources.Length; i++)
        {
            this.SFXAudioSources[i].SetVolume(volume);
        }
    }

    public void Play(BACKGROUNDSOUND sound)
    {
        this.BackGroundAudioSource.clip = this.GetAudio(sound);
        this.BackGroundAudioSource.Play();
    }
    public void Play(SKILLBOOKSOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }
    public void Play(MAPSOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }
    public void Play(BATTLESOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }
    public void Play(EVENTSOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }
    public void Play(SHOPSOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }
    public void Play(REWARDSOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }
    public void Play(CARDSOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }

    public void Play(RESTSOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }

    public void Play(DEBUFFSOUND sound)
    {
        this.SFXPlay(this.GetAudio(sound));
    }
    public void SFXPlay(AudioClip clip)
    {
        for (int i = 0; i < this.SFXAudioSources.Length; i++)
        {
            if (!this.SFXAudioSources[i].isPlaying)
            {
                this.SFXAudioSources[i].Play(clip);
                break;
            }
        }
    }

    public AudioClip GetAudio(BACKGROUNDSOUND sound)
    {
        return this.BackGroundAudio[(int)sound];
    }
    public AudioClip GetAudio(SKILLBOOKSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case SKILLBOOKSOUND.OPEN_BOOK:
                clip = this.sounds.skillbook_openBook;
                break;
            case SKILLBOOKSOUND.CLOSE_BOOK:
                clip = this.sounds.skillbook_closeBook;
                break;
            case SKILLBOOKSOUND.CARD_ON_BOOK:
                clip = this.sounds.skillbook_cardOnBook;
                break;
            case SKILLBOOKSOUND.CARD_NUM_UP_DOWN:
                clip = this.sounds.skillbook_cardNumUpDown;
                break;
            case SKILLBOOKSOUND.TURN_PAGE:
                clip = this.sounds.skillbook_turnPage;
                break;
            default:
                clip = this.sounds.skillbook_openBook;
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
                clip = this.sounds.map_choiceField;
                break;
            case MAPSOUND.OPEN_DEBUFFWINDOW:
                clip = this.sounds.map_openDebuffWindow;
                break;
            case MAPSOUND.CHOICE_DEBUFF:
                clip = this.sounds.map_choiceDebuff;
                break;
            case MAPSOUND.SHOW_DEBUFF_BUTTON:
                clip = this.sounds.map_showDebuffButton;
                break;
            default:
                clip = this.sounds.map_choiceField;
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
                clip = this.sounds.battle_heal;
                break;
            case BATTLESOUND.CARD_DRAW:
                clip = this.sounds.battle_cardDraw;
                break;
            case BATTLESOUND.SHELD:
                clip = this.sounds.battle_sheld;
                break;
            case BATTLESOUND.HIT:
                clip = this.sounds.battle_hit;
                break;
            case BATTLESOUND.TURN_START:
                clip = this.sounds.battle_turnStart;
                break;
            case BATTLESOUND.TURN_END:
                clip = this.sounds.battle_turnEnd;
                break;
            case BATTLESOUND.GAME_WIN:
                clip = this.sounds.battle_gameWin;
                break;
            case BATTLESOUND.GAME_FAILD:
                clip = this.sounds.battle_gameFaild;
                break;
            default:
                clip = this.sounds.battle_heal;
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
                clip = this.sounds.event_choiceMouseUp;
                break;
            case EVENTSOUND.CHOICE_BUTTON:
                clip = this.sounds.event_choiceButton;
                break;
            default:
                clip = this.sounds.event_choiceButton;
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
                clip = this.sounds.shop_buyitem;
                break;
            case SHOPSOUND.SOLDOUT:
                clip = this.sounds.shop_soldOut;
                break;
            case SHOPSOUND.THROWINGOBJ:
                clip = this.sounds.shop_throwingObj;
                break;
            case SHOPSOUND.IN_TOPBAR_ICON:
                clip = this.sounds.shop_inTopBarIcon;
                break;
            default:
                clip = this.sounds.shop_buyitem;
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
                clip = this.sounds.reward_showRewardWindow;
                break;
            case REWARDSOUND.GETQUESTION:
                clip = this.sounds.reward_getQuestion;
                break;
            case REWARDSOUND.GETCARDPIECE:
                clip = this.sounds.reward_getCardPiece;
                break;
            case REWARDSOUND.LOSTHEAL:
                clip = this.sounds.reward_lostHeal;
                break;
            case REWARDSOUND.SHOW_REWARD_BUTTON:
                clip = this.sounds.reward_showRewardButton;
                break;
            default:
                clip = this.sounds.reward_getQuestion;
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
                clip = this.sounds.card_upCard;
                break;
            case CARDSOUND.GO_BACK:
                clip = this.sounds.card_goback;
                break;
            case CARDSOUND.Shuffling:
                clip = this.sounds.card_shuffling;
                break;
            default:
                clip = this.sounds.card_upCard;
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
                clip = this.sounds.rest_heal;
                break;
            default:
                clip = this.sounds.rest_heal;
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
                clip = this.sounds.debuff_openbar;
                break;
            case DEBUFFSOUND.CLOSE_BAR:
                clip = this.sounds.debuff_closebar;
                break;
            default:
                clip = this.sounds.debuff_openbar;
                break;
        }
        return clip;
    }
}
