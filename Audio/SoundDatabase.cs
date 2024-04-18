using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//это класс-баzа, который содержит звуки для кнопок и др.
public class SoundDatabase : MonoBehaviour
{
    static AudioClip[] _buttonSounds;
    private void Start()
    {
        Events.onButtonSoundAdd?.Invoke(_buttonSounds);
    }
    public void Initialize()
    {
        _buttonSounds = Resources.LoadAll<AudioClip>("MusicAndSounds/ButtonSounds");
    }

    private void OnApplicationQuit()
    {
        Events.onButtonSoundAdd=null;
    }
}
