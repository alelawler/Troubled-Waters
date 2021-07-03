using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    public SceneLoader mySceneLoader;
    public float volume;


    private void Start()
    {
       myVideoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Intro.mp4");
       myVideoPlayer.loopPointReached += EndReached;
        myVideoPlayer.SetDirectAudioVolume(0,volume);
       myVideoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        mySceneLoader.LoadScene("Level1Screen");
    }
}
