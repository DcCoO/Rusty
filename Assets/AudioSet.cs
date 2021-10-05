using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSet : MonoBehaviour
{
    [SerializeField] AudioSource[] sources;
    int index = 0;
    
    public void Play(AudioClip clip, bool loop = false)
    {
        if (loop)
        {
            sources[index].loop = true;
            sources[index].clip = clip;
            sources[index].Play();
        }
        else
        {
            sources[index].loop = false;
            sources[index].PlayOneShot(clip);
        }
        index = (index + 1) % sources.Length;
    }
}
