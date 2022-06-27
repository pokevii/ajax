using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public string soundName;
    public bool fadeOut;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (fadeOut)
            {
                audioManager.FadeOut(soundName);
            } else
            {
                audioManager.FadeIn(soundName);
            }
        }
    }
}
