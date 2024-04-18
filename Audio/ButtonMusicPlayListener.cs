using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ����� �� ������� ������� �� ������ ������������ ���� � �������������
public class ButtonMusicPlayListener : MonoBehaviour
{
    AudioClip _buttonSoundEffect;
    private void Awake()
    {
        Events.onButtonSoundAdd += Initialize;
    }

    void Initialize(AudioClip[] sounds)
    {
        _buttonSoundEffect = sounds[Random.Range(0, sounds.Length)];
        GetComponent<Button>().onClick.AddListener(PlayEffect);
    }

    void PlayEffect()
    {
        Events.onPlaySoundEffect?.Invoke(_buttonSoundEffect);
    }
}
