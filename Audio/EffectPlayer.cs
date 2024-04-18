using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ����� ��������� �� ������� ���� � ����������� ���
public class EffectPlayer : MonoBehaviour
{
    
    AudioSource _audioSource;
    private void Awake()
    {
        Events.onPlaySoundEffect = null;
        _audioSource = GetComponent<AudioSource>();
        Events.onPlaySoundEffect = PlayEffect;
    }

    void PlayEffect(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
