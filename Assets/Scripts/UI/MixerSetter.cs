using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MixerSetter : MonoBehaviour
{
    [SerializeField]
    private AudioMixer amg;
    [SerializeField]
    private string audioMixerName;
    [SerializeField]
    private Slider slider1;
    public void UpdateAudioMixerLevel()
    {
        amg.SetFloat(audioMixerName, ((slider1.value * 100f) - 80f));
    }
}
