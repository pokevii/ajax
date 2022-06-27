using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimatorEvents : MonoBehaviour
{
    //private float maxTime;
    private float timer = 0;
    private bool sceneChanging = false;
    private string queuedScene;
    private AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void PlaySound(string sound)
    {
        audioManager.Play(sound);
    }

    public void StopSound(string sound)
    {
        audioManager.Stop(sound);
    }

    public void ChangeScene(string sceneToGoTo)
    {
        SceneManager.LoadScene(sceneToGoTo, LoadSceneMode.Single);
    }

    public void SendTriggerToGameObject(string triggerName)
    {
        gameObject.GetComponent<Animator>().SetTrigger(triggerName);
    }

    public void FadeInThenSceneChange(string sceneToGoTo)
    {
        timer = 2.0f;
        sceneChanging = true;
        queuedScene = sceneToGoTo;
        GetComponent<Animator>().SetTrigger("fadeIn");
    }

    public void FadeOutAudio(string sound)
    {
        audioManager.FadeOut(sound);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (sceneChanging)
        {
            if (timer <= 0)
            {
                Debug.Log("we going");
                SceneManager.LoadScene(queuedScene, LoadSceneMode.Single);
                sceneChanging = false;
            }
            timer -= Time.deltaTime;
        }
    }
}
