using System.Collections;
using UnityEngine;

public class SFXSound : MonoBehaviour
{
    AudioSource audioSource;

    public bool isPlaying { get { return this.audioSource.isPlaying; } }

    private void Awake()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        this.audioSource.clip = clip;
        this.audioSource.Play();
        this.StartCoroutine(this.PlayCoroutine());
    }

    IEnumerator PlayCoroutine()
    {
        while (true)
        {
            if (!this.audioSource.isPlaying)
            {
                this.audioSource.clip = null;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetVolume(float volume)
    {
        this.audioSource.volume = volume;
    }
}
