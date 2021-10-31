using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BACKGROUNDSOUND { INTRO, TUTORIAL, MAP, BATTLE, EVENT, SHOP, REST, BOSS, ENDING }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst = null;

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
    public SFXSound[] SFXAudioSource;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SFXPlay(SFXAudio[0]);
    }

    public void BackGroundPlay(BACKGROUNDSOUND sound)
    {
        BackGroundAudioSource.clip = GetAudio(sound);
        BackGroundAudioSource.Play();
    }
    public void SFXPlay(AudioClip clip)
    {
        for (int i = 0; i < SFXAudioSource.Length; i++)
        {
            if (!SFXAudioSource[i].isPlaying)
            {
                SFXAudioSource[i].Play(clip);
                break;
            }
        }
    }

    public AudioClip GetAudio(BACKGROUNDSOUND sound)
    {
        return BackGroundAudio[(int)sound];
    }
}
