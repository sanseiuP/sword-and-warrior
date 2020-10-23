using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }//单例模式

    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }


    public void AudioPlay(AudioClip clip)//播放音频相关
    {
        source.PlayOneShot(clip);//只播放一次
    }
}
