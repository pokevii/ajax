using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    //Change music based on scene
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        switch (scene.name)
        {
            case "TitleScene":
                Debug.Log("hello");
                Play("intro");
                break;
            case "Forest":
                Stop("intro");
                FadeIn("forest");
                break;
            case "Ruins":
                Stop("forest");
                Play("ruins");
                break;
            case "Corruption":
                Stop("ruins");
                Play("corruption");
                break;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.Stop();
    }

    public void FadeIn(string name)
    {
        StartCoroutine(FadeInCoroutine(name));
    } 

    public void FadeOut(string name)
    {
        StartCoroutine(FadeOutCoroutine(name));
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator FadeInCoroutine(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound {name} not found when calling FadeOut. Did you misspell it?");
            yield return null;
        }

        s.volume = 0;
        s.source.Play();

        while (s.source.volume < s.volume)
        {
            s.source.volume += 0.001f;
            yield return null;
        }
    }

    IEnumerator FadeOutCoroutine(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound {name} not found when calling FadeOut. Did you misspell it?");
            yield return null;

        }

        s.source.volume = s.volume;

        while (s.source.volume > 0)
        {
            s.source.volume -= 0.003f;
            yield return null;
        }
        s.source.Stop();
    }
}
