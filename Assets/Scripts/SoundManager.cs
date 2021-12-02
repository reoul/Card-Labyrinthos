using System;
using UnityEngine;

public enum BACKGROUNDSOUND
{
    Intro,
    Tutorial,
    Map,
    Battle,
    Event,
    Shop,
    Rest,
    Boss,
    Ending
}

public enum SKILLBOOKSOUND
{
    OpenBook,
    CloseBook,
    CardONBook,
    CardNumUpDown,
    TurnPage
}

public enum MAPSOUND
{
    ChoiceField,
    OpenDebuffWindow,
    ChoiceDebuff,
    ShowDebuffButton
}

public enum BATTLESOUND
{
    Heal,
    CardDraw,
    Shield,
    Hit,
    TurnStart,
    TurnEnd,
    GameWin,
    GameFaild
}

public enum EVENTSOUND
{
    ChoiceMouseup,
    ChoiceButton
}

public enum SHOPSOUND
{
    Buy,
    SoldOut,
    ThrowingObj,
    InTopBarIcon
}

public enum REWARDSOUND
{
    ShowRewardWindow,
    GetQuestion,
    GetCardPiece,
    LostHeal,
    ShowRewardButton
}

public enum CARDSOUND
{
    UpCard,
    GOBack,
    Shuffling
}

public enum RESTSOUND
{
    Heal
}

public enum DEBUFFSOUND
{
    OpenBar,
    CloseBar
}


[Serializable]
public class Sounds
{
    [Header("스킬북")] public AudioClip skillbook_openBook;
    public AudioClip skillbook_closeBook;
    [Header("카드 스킬북에 올려놓는 소리")] public AudioClip skillbook_cardOnBook;
    public AudioClip skillbook_cardNumUpDown;
    public AudioClip skillbook_turnPage;
    [Header("지도")] public AudioClip map_choiceField;
    public AudioClip map_openDebuffWindow;
    public AudioClip map_choiceDebuff;
    public AudioClip map_showDebuffButton;
    [Header("전투")] public AudioClip battle_heal;
    public AudioClip battle_cardDraw;
    public AudioClip battle_sheld;
    public AudioClip battle_hit;
    public AudioClip battle_turnStart;
    public AudioClip battle_turnEnd;
    public AudioClip battle_gameWin;
    public AudioClip battle_gameFaild;
    [Header("이벤트")] public AudioClip event_choiceMouseUp;
    public AudioClip event_choiceButton;
    [Header("상점")] public AudioClip shop_buyitem;
    public AudioClip shop_soldOut;
    public AudioClip shop_throwingObj;
    public AudioClip shop_inTopBarIcon;
    [Header("보상창")] public AudioClip reward_showRewardWindow;
    public AudioClip reward_getQuestion;
    public AudioClip reward_getCardPiece;
    public AudioClip reward_lostHeal;
    public AudioClip reward_showRewardButton;
    [Header("카드")] public AudioClip card_upCard;
    public AudioClip card_goback;
    public AudioClip card_shuffling;
    [Header("휴식방")] public AudioClip rest_heal;
    [Header("저주바")] public AudioClip debuff_openbar;
    public AudioClip debuff_closebar;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private Sounds sounds;

    private void Awake()
    {
        CheckExistInstanceAndDestroy(this);
    }

    public AudioClip[] BackGroundAudio;

    public AudioSource BackGroundAudioSource;
    public SFXSound[] SFXAudioSources;

    public void SetBGMVolume(float volume)
    {
        BackGroundAudioSource.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        foreach (SFXSound sfxAudio in SFXAudioSources)
        {
            sfxAudio.SetVolume(volume);
        }
    }

    public void Play(BACKGROUNDSOUND sound)
    {
        BackGroundAudioSource.clip = GetAudio(sound);
        BackGroundAudioSource.Play();
    }

    public void Play(SKILLBOOKSOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    public void Play(MAPSOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    public void Play(BATTLESOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    public void Play(EVENTSOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    public void Play(SHOPSOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    public void Play(REWARDSOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    public void Play(CARDSOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    public void Play(RESTSOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    public void Play(DEBUFFSOUND sound)
    {
        SfxPlay(GetAudio(sound));
    }

    private void SfxPlay(AudioClip clip)
    {
        foreach (SFXSound sfxAudio in SFXAudioSources)
        {
            if (!sfxAudio.IsPlaying)
            {
                sfxAudio.Play(clip);
                break;
            }
        }
    }

    private AudioClip GetAudio(BACKGROUNDSOUND sound)
    {
        return BackGroundAudio[(int) sound];
    }

    private AudioClip GetAudio(SKILLBOOKSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case SKILLBOOKSOUND.OpenBook:
                clip = sounds.skillbook_openBook;
                break;
            case SKILLBOOKSOUND.CloseBook:
                clip = sounds.skillbook_closeBook;
                break;
            case SKILLBOOKSOUND.CardONBook:
                clip = sounds.skillbook_cardOnBook;
                break;
            case SKILLBOOKSOUND.CardNumUpDown:
                clip = sounds.skillbook_cardNumUpDown;
                break;
            case SKILLBOOKSOUND.TurnPage:
                clip = sounds.skillbook_turnPage;
                break;
            default:
                clip = sounds.skillbook_openBook;
                break;
        }

        return clip;
    }

    private AudioClip GetAudio(MAPSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case MAPSOUND.ChoiceField:
                clip = sounds.map_choiceField;
                break;
            case MAPSOUND.OpenDebuffWindow:
                clip = sounds.map_openDebuffWindow;
                break;
            case MAPSOUND.ChoiceDebuff:
                clip = sounds.map_choiceDebuff;
                break;
            case MAPSOUND.ShowDebuffButton:
                clip = sounds.map_showDebuffButton;
                break;
            default:
                clip = sounds.map_choiceField;
                break;
        }

        return clip;
    }

    private AudioClip GetAudio(BATTLESOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case BATTLESOUND.Heal:
                clip = sounds.battle_heal;
                break;
            case BATTLESOUND.CardDraw:
                clip = sounds.battle_cardDraw;
                break;
            case BATTLESOUND.Shield:
                clip = sounds.battle_sheld;
                break;
            case BATTLESOUND.Hit:
                clip = sounds.battle_hit;
                break;
            case BATTLESOUND.TurnStart:
                clip = sounds.battle_turnStart;
                break;
            case BATTLESOUND.TurnEnd:
                clip = sounds.battle_turnEnd;
                break;
            case BATTLESOUND.GameWin:
                clip = sounds.battle_gameWin;
                break;
            case BATTLESOUND.GameFaild:
                clip = sounds.battle_gameFaild;
                break;
            default:
                clip = sounds.battle_heal;
                break;
        }

        return clip;
    }

    private AudioClip GetAudio(EVENTSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case EVENTSOUND.ChoiceMouseup:
                clip = sounds.event_choiceMouseUp;
                break;
            case EVENTSOUND.ChoiceButton:
                clip = sounds.event_choiceButton;
                break;
            default:
                clip = sounds.event_choiceButton;
                break;
        }

        return clip;
    }

    private AudioClip GetAudio(SHOPSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case SHOPSOUND.Buy:
                clip = sounds.shop_buyitem;
                break;
            case SHOPSOUND.SoldOut:
                clip = sounds.shop_soldOut;
                break;
            case SHOPSOUND.ThrowingObj:
                clip = sounds.shop_throwingObj;
                break;
            case SHOPSOUND.InTopBarIcon:
                clip = sounds.shop_inTopBarIcon;
                break;
            default:
                clip = sounds.shop_buyitem;
                break;
        }

        return clip;
    }

    private AudioClip GetAudio(REWARDSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case REWARDSOUND.ShowRewardWindow:
                clip = sounds.reward_showRewardWindow;
                break;
            case REWARDSOUND.GetQuestion:
                clip = sounds.reward_getQuestion;
                break;
            case REWARDSOUND.GetCardPiece:
                clip = sounds.reward_getCardPiece;
                break;
            case REWARDSOUND.LostHeal:
                clip = sounds.reward_lostHeal;
                break;
            case REWARDSOUND.ShowRewardButton:
                clip = sounds.reward_showRewardButton;
                break;
            default:
                clip = sounds.reward_getQuestion;
                break;
        }

        return clip;
    }

    private AudioClip GetAudio(CARDSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case CARDSOUND.UpCard:
                clip = sounds.card_upCard;
                break;
            case CARDSOUND.GOBack:
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

    private AudioClip GetAudio(RESTSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case RESTSOUND.Heal:
                clip = sounds.rest_heal;
                break;
            default:
                clip = sounds.rest_heal;
                break;
        }

        return clip;
    }

    private AudioClip GetAudio(DEBUFFSOUND sound)
    {
        AudioClip clip;
        switch (sound)
        {
            case DEBUFFSOUND.OpenBar:
                clip = sounds.debuff_openbar;
                break;
            case DEBUFFSOUND.CloseBar:
                clip = sounds.debuff_closebar;
                break;
            default:
                clip = sounds.debuff_openbar;
                break;
        }

        return clip;
    }
}
