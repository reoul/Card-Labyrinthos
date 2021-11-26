using System.Collections;
using UnityEngine;

public class SFXSound : MonoBehaviour
{
    private AudioSource audioSource;

    public bool isPlaying { get { return audioSource.isPlaying; } }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = null;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
