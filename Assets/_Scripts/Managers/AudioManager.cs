using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

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

    [Header("SfxPool")]
    public List<PitchGroup> groups = new List<PitchGroup>();
    public AudioMixer mixer;

    public PitchGroup GetAvailableGroup()
    {
        foreach (var g in groups)
        {
            if (!g.inUse)
            {
                g.inUse = true;
                return g;
            }
        }

        // No free group? Just reuse the first one.
        // (Optional: Expand pool or add round-robin logic)
        return groups[0];
    }

    public void ReleaseGroup(PitchGroup g)
    {
        g.inUse = false;
    }

    public void PlaySound(AudioSource source, float pitch)
    {
        var g = GetAvailableGroup();

        source.outputAudioMixerGroup = g.mixerGroup;

        mixer.SetFloat(g.exposedPitchParam, pitch);

        source.Play();

        StartCoroutine(ReleaseAfter(source, g));
    }

    public void PlaySound(AudioSource source, float pitch, AudioClip clip)
    {
        var g = GetAvailableGroup();

        source.outputAudioMixerGroup = g.mixerGroup;

        mixer.SetFloat(g.exposedPitchParam, pitch);

        source.PlayOneShot(clip);

        StartCoroutine(ReleaseAfter(source, g));
    }

    private System.Collections.IEnumerator ReleaseAfter(AudioSource src, PitchGroup group)
    {
        yield return new WaitWhile(() => src != null && src.isPlaying);
        ReleaseGroup(group);
    }

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
        _sfxSource.PlayOneShot(_sfxClips[index]);
    }

    public void PlaySFX(AudioClip clip)
    {
        _mixer.SetFloat("sfxPitch", 1f);
        _sfxSource.PlayOneShot(clip);
    }

    public void PlaySFXPitched(AudioClip clip, float pitch)
    {
        _mixer.SetFloat("sfxPitch", pitch);
        _sfxSource.PlayOneShot(clip);
    }

    public void SetMasterVolume(float volume) => _mixer.SetFloat("masterVolume", volume);

    public void SetMusicVolume(float volume) => _mixer.SetFloat("musicVolume", volume);

    public void SetSFXVolume(float volume) => _mixer.SetFloat("sfxVolume", volume);

    public float GetMasterVolume()
    {
        _mixer.GetFloat("masterVolume", out float volume);
        return volume;
    }

    public float GetMusicVolume() 
    {
        _mixer.GetFloat("musicVolume", out float volume);
        return volume;
    }

    public float GetSFXVolume() {
        _mixer.GetFloat("sfxVolume", out float volume);
        return volume;
    }
}
[System.Serializable]
public class PitchGroup
{
    public AudioMixerGroup mixerGroup;
    public string exposedPitchParam; // e.g. "Pitch_1"
    [HideInInspector] public bool inUse;
}
