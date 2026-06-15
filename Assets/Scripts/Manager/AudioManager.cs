using System.Collections.Generic;
using UnityEngine;

public enum AudioClips
{
    OpenChest,
    IslandComplete,
    ButtonClick,
    NewHighscore,
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [SerializeField] private AudioClip openChestSound;
    [SerializeField] private AudioClip islandCompleteSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip newHighscoreSound;

    private Dictionary<AudioClips, AudioClip> audioClips = new();
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            audioClips.Add(AudioClips.OpenChest, openChestSound);
            audioClips.Add(AudioClips.IslandComplete, islandCompleteSound);
            audioClips.Add(AudioClips.ButtonClick, buttonClickSound);
            audioClips.Add(AudioClips.NewHighscore, newHighscoreSound);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySfx(AudioClips clip)
    {
        Debug.Log(clip);
        AudioClip sound = audioClips[clip];
        sfxSource.PlayOneShot(sound);
    }
}