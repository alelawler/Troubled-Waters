using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour
{
    private AudioSource myAudioSource;

    private static MenuMusicManager instance = null;
    public static MenuMusicManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else
            instance = this;
        DontDestroyOnLoad(transform.gameObject);
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (myAudioSource.isPlaying) 
            return;
        myAudioSource.Play();
    }

    public void StopMusic()
    {
        myAudioSource.Stop();
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "IntroCinematic")
            Destroy(this.gameObject);
    }
}
