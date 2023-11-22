using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSoundHelper : MonoBehaviour
{
    public AudioPoolInfo sound;
    public void PlaySoundThroughPool()
    {
        AudioSourceProvider.instance.PlayClipOnSource(sound);
    }
}
