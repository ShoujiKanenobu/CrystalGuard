using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct AudioPoolInfo
{
    public AudioClip clip;
    public float volume;
    public AudioMixerGroup mixGroup;
}

public class AudioSourceProvider : MonoBehaviour
{

    public static AudioSourceProvider instance;

    public List<AudioSource> pool = new List<AudioSource>();

    [SerializeField]
    private int initialPoolSize;
    [SerializeField]
    private int poolLimit;
    void Start()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        AudioSource asource;
        for(int i = 0; i < initialPoolSize; i++)
        {
            asource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            pool.Add(asource);
        }
    }

    public void PlayClipOnSource(AudioPoolInfo info)
    {
        int openSource = -1;
        for(int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].isPlaying)
            {
                openSource = i;
                break;
            }   
        }

        if (openSource == -1 && pool.Count <= poolLimit)
        {
            AudioSource asource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            pool.Add(asource);
            openSource = pool.Count - 1;
        }

        if (openSource == -1)
            return;

        pool[openSource].volume = info.volume;
        pool[openSource].clip = info.clip;
        pool[openSource].outputAudioMixerGroup = info.mixGroup;
        pool[openSource].Play();
    }
}
