using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;
    [SerializeField]private AudioClip onBegingSceneBGM;
    [SerializeField]private AudioClip onGameSceneBGM;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.mute = !GameDataManager.Instance.MusicData.isMusicOpen;
        audioSource.volume = GameDataManager.Instance.MusicData.musicVolume;
    }
    public void ChangeBGMByScene()
    {
        if (SceneManager.GetActiveScene().name=="BeginScene")
        {
            audioSource.clip = onBegingSceneBGM;
        }
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            audioSource.clip = onGameSceneBGM;
        }
        audioSource.Play();
    }
    public void ChangeMusicVolume(float value)
    {
        audioSource.volume = Mathf.Clamp01(value);
    }
    public void ChangMusicState(bool value)
    {
        audioSource.mute = !value;
    }
    public void SetSoundState(AudioSource soundAudio)
    {
        MusicData musicData = GameDataManager.Instance.MusicData;
        if(soundAudio != null)
        {
            soundAudio.mute = !musicData.isSoundOpen;
            soundAudio.volume = Mathf.Clamp01(musicData.soundVolume);
        }
    }
}
