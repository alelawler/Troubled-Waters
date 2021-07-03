using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHelper : MonoBehaviour
{
    static private SoundHelper myInstance;

    public AudioSource myAudioSource;

    private void Awake()
    {
        if (myInstance == null)
        {
            myInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

    }
    static public void PlaySound(AudioClip clipToPlay, float volume = 0.5f)
    {
        if (myInstance != null)
        {
            myInstance.myAudioSource.PlayOneShot(clipToPlay, volume);
        }
    }



}
