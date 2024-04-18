using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AudioPlayer : MonoBehaviour
{
    public static AudioClip[] musicClipList;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioSource effectPlayer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    private float _musicVolume;
    private float _effectVolume;
    private int _previousIndex;
    private double _endTime;
    
    void Awake()
    {
        Events.onSetAudioSettings = null;
        StartCoroutine(Initialize());
    }
    
    IEnumerator Initialize()
    {
        while (JSONTest.UserLocalDataObject == null)
        {
            yield return null;
        }
        while (JSONTest.UserLocalDataObject.LocalUserInfo == null)
        {
            yield return null;
        }
        Events.onSetAudioSettings += SetSettings;
    }

    int PlayNext(int index)
    {
        int newIndex;
        do
        {
            newIndex = UnityEngine.Random.Range((int)0, (int)musicClipList.Length);
        } while (newIndex == index);

        audioPlayer.PlayOneShot(musicClipList[newIndex]);
        _endTime = AudioSettings.dspTime + musicClipList[newIndex].length+2f;
        audioPlayer.PlayScheduled(_endTime);

        return newIndex;
    }
    IEnumerator PlayAmbient()
    {
        Debug.Log("-----------------------Music Corutine started");
        while (gameObject.activeSelf)
        {
            yield return new WaitForSecondsRealtime(musicClipList[_previousIndex].length-10f);
            Debug.Log("-------------------------Make lower volume");
            while (audioPlayer.volume > 0)
            {
                audioPlayer.volume -= 0.025f;
                yield return new WaitForSeconds(0.25f);
            }
            
            yield return new WaitForSecondsRealtime(1f);
            audioPlayer.Stop();
            Debug.Log("-----------------------New song started");
            _previousIndex=PlayNext(_previousIndex);
            Debug.Log("-------------------------Make higher volume");
            while (audioPlayer.isPlaying && audioPlayer.volume < _musicVolume)
            {
                audioPlayer.volume += 0.025f;
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    void SetSettings()
    {
        Debug.Log("---------------------------------------"+JSONTest.UserLocalDataObject.LocalUserInfo.sound_volume);
        Debug.Log("---------------------------------------" + JSONTest.UserLocalDataObject.LocalUserInfo.music_volume);
        effectSlider.value = JSONTest.UserLocalDataObject.LocalUserInfo.sound_volume;
        musicSlider.value = JSONTest.UserLocalDataObject.LocalUserInfo.music_volume;
        audioPlayer.mute = !JSONTest.UserLocalDataObject.LocalUserInfo.music_state;
        effectPlayer.mute = !JSONTest.UserLocalDataObject.LocalUserInfo.sound_state;
        
        musicClipList = Resources.LoadAll<AudioClip>("MusicAndSounds/Ambient");
        audioPlayer = gameObject.GetComponent<AudioSource>();

        _previousIndex = Random.Range(0, musicClipList.Length);
        audioPlayer.PlayOneShot(musicClipList[_previousIndex]);
        _endTime= AudioSettings.dspTime + musicClipList[0].length+2f;

        audioPlayer.PlayScheduled(_endTime);
        StartCoroutine(PlayAmbient());
    
        Events.onPlaySoundEffect = null;
        Events.onPlaySoundEffect = PlayEffect;
        
        SetMusicVolume();
        SetEffectVolume();
    }
    
    void PlayEffect(AudioClip clip)
    {
        effectPlayer.PlayOneShot(clip);
    }

    public void SetEffectVolume()
    {
        effectPlayer.volume = effectSlider.value;
        JSONTest.UserLocalDataObject.LocalUserInfo.sound_volume = effectPlayer.volume;
    }

    public void SetMusicVolume()
    {
        _musicVolume = musicSlider.value;
        audioPlayer.volume = _musicVolume;
        JSONTest.UserLocalDataObject.LocalUserInfo.music_volume = audioPlayer.volume;
    }

    public void MuteEffects()
    {
        effectPlayer.mute = (effectPlayer.mute == false);
        JSONTest.UserLocalDataObject.LocalUserInfo.sound_state = !effectPlayer.mute;
    }

    public void MuteMusic()
    {
        audioPlayer.mute = (audioPlayer.mute == false);
        JSONTest.UserLocalDataObject.LocalUserInfo.music_state = !audioPlayer.mute;

    }

    private void OnApplicationQuit()
    {
        Debug.Log("+++++++++++++++++++++++++++++++++++++++++"+JSONTest.UserLocalDataObject.LocalUserInfo.sound_volume);
        Debug.Log("++++++++++++++++++++++++++++++++++++++++" + JSONTest.UserLocalDataObject.LocalUserInfo.music_volume);
        Events.onSetAudioSettings = null;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            audioPlayer.mute = false;
            effectPlayer.mute = false;
        }
        else
        {
            //audioPlayer.mute = !JSONTest.UserLocalDataObject.LocalUserInfo.music_state;
            //effectPlayer.mute = !JSONTest.UserLocalDataObject.LocalUserInfo.sound_state;
            audioPlayer.mute = true;
            effectPlayer.mute = true;
        }
    }
}
