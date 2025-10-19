using UnityEngine;

// TODO: Evgeniy - Refactor this
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayMusic(int index)
    {
        musicSource.clip = musicClips[index];
        musicSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(int index)
    {
        sfxSource.clip = sfxClips[index];
        sfxSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

}
