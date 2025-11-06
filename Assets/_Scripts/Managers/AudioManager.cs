using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] _musicClips;
    [SerializeField] private AudioClip[] _sfxClips;

    [Header("Mixers")]
    [SerializeField] private AudioMixer _mixer;

    public static AudioManager instance;

    public AudioClip[] MusicClips => _musicClips;
    public AudioClip[] SFXClips => _sfxClips;
    public AudioMixer Mixer => _mixer;

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

    public void SetMasterVolume(float volume) => _mixer.SetFloat("masterVolume", volume);

    public void SetMusicVolume(float volume) => _mixer.SetFloat("musicVolume", volume);

    public void SetSFXVolume(float volume) => _mixer.SetFloat("sfxVolume", volume);

    public void GetMasterVolume(out float volume) => _mixer.GetFloat("masterVolume", out volume);

    public void GetMusicVolume(out float volume) => _mixer.GetFloat("musicVolume", out volume);

    public void GetSFXVolume(out float volume) => _mixer.GetFloat("sfxVolume", out volume);
}
