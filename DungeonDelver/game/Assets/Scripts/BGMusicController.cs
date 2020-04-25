using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusicController : MonoBehaviour
{
    public AudioClip BGMusic;
    public AudioClip BossMusic;
    public AudioClip WinMusic;

    private AudioSource source;
    private bool boss;
    private bool win;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = BGMusic;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name.CompareTo("Boss") == 0 && !boss)
        {
            source.clip = BossMusic;
            source.volume = 0.45f;
            source.Play();
            boss = true;
        }

        if(SceneManager.GetActiveScene().name.CompareTo("Win") == 0 && !win)
        {
            source.clip = WinMusic;
            source.volume = 0.65f;
            source.Play();
            win = true;
        }
    }
}
