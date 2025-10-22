using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip[] _musicClips;
    [SerializeField] private AudioClip[] _sfxClips;

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
        _musicSource.clip = _musicClips[index];
        _musicSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySFX(int index)
    {
        _sfxSource.clip = _sfxClips[index];
        _sfxSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }

    public AudioClip[] MusicClips => _musicClips;
    public AudioClip[] SFXClips => _sfxClips;
}
