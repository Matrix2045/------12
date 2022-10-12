using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static AudioManager Instance;
    private static UnityEngine.Object lockobj = new UnityEngine.Object();
    public static AudioManager GetInstance
    {
        get
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<AudioManager>() as AudioManager;
                }
            }
            return Instance;
        }
    }
    private bool isPlaying = true;
    private void Start()
    {
        //Application.ExternalCall("MusicLoad", "TemplateData/music/Test.mp3");
    }
    /// <summary>
    /// 音频开关
    /// </summary>
    // Start is called before the first frame update
    public void PlayOrPause()
    {
        if (isPlaying)
        {
            Application.ExternalCall("MusicPause");
            isPlaying = false;
        }
        else
        {
            Application.ExternalCall("MusicPlay");
            isPlaying = true;
        }
        //if (GetComponent<AudioSource>().isPlaying)
        //{
        //    GetComponent<AudioSource>().Pause();
        //}
        //else {
        //    GetComponent<AudioSource>().Play();
        //}
    }
}
