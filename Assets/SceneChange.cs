using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public string sceneToGoTo;
    private bool fadeIn = false;
    public Color fadeInColor;
    public Image fadeImage;
    private float alpha;
    public float fadeInSpeed = 0.02f;
    // Start is called before the first frame update

    private void Start()
    {
        alpha = 0;
    }

    private void Update()
    {
        if (fadeIn)
        {
            alpha += fadeInSpeed * Time.deltaTime;
            fadeImage.color = new Color(fadeInColor.r, fadeInColor.g, fadeInColor.b, alpha);
        }
        if (fadeImage.color.a >= 1)
        {
            fadeImage.color = new Color(fadeInColor.r, fadeInColor.g, fadeInColor.b, 1);
            fadeIn = false;
            ChangeScene();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fadeImage.color = new Color(fadeInColor.r, fadeInColor.g, fadeInColor.b, 0);
            fadeIn = true;
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneToGoTo, LoadSceneMode.Single);
    }
}
