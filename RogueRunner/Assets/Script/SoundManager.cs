using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;

    public void setLevel(float val)
    {
        float vol = slider.value;

        //"BGM"�� Mixer�� �׷��
        if (vol == -40f) audioMixer.SetFloat("BGM", -80f);
        else audioMixer.SetFloat("BGM", Mathf.Log10(vol) * 20);
    }

    //���Ұ� ����
    public void ToggleMute()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
}
