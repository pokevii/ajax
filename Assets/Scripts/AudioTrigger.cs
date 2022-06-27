using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public string soundName;
    public bool fade;
    public bool play;
    public bool stop;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (stop)
            {
                if (fade)
                {
                    audioManager.FadeOut(soundName);
                } else
                {
                    audioManager.Stop(soundName);
                }
            } 
            else if (play)
            {
                if (fade)
                {
                    audioManager.FadeIn(soundName);
                } else
                {
                    audioManager.Play(soundName);
                }
            }

        }
    }
}
