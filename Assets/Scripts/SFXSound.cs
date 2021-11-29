using System.Collections;
using UnityEngine;

public class SFXSound : MonoBehaviour
{
    private AudioSource audioSource;

    private static WaitForSeconds delay005 = new WaitForSeconds(0.05f);

    public bool IsPlaying
    {
        get { return audioSource.isPlaying; }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    /// <param name="clip">효과음</param>
    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(PlayCoroutine());
    }

    /// <summary>
    /// 효과음 재생이 종료되면 clip을 null로 바꿔준다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayCoroutine()
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = null;
                break;
            }

            yield return delay005;
        }
    }

    /// <summary>
    /// 볼륨을 설정한다.
    /// </summary>
    /// <param name="volume">볼륨 ( 0 ~ 1 )</param>
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
