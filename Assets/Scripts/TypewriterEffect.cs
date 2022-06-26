using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI dialogue;
    public float waitTime = 0.03f;
    public AudioSource[] source;
    public Animator avatarAnimator;
    private string tempDialogue;
    private bool dialogueCalledOnce;
    // Start is called before the first frame update
    void Start()
    {
        dialogueCalledOnce = false;
    }

    private void OnEnable()
    {
        dialogueCalledOnce = false;
    }

    public void StartTypewriter()
    {
        if (dialogueCalledOnce)
        {
            avatarAnimator.SetTrigger("overwrite");
        }
        Debug.Log("Typewriter Started");
        StopAllCoroutines();
        tempDialogue = dialogue.text;
        dialogue.text = "";
        StartCoroutine(Typewriter());
        dialogueCalledOnce = true;
    }

    private int randSound;
    IEnumerator Typewriter()
    {
        for(int i=0; i < tempDialogue.Length; i++)
        {
            randSound = Random.Range(0, 4);
            dialogue.text += tempDialogue[i];
            switch (tempDialogue[i])
            {
                case ' ':
                    break;
                case ',':
                case ':':
                    yield return new WaitForSeconds(waitTime * 1.5f);
                    break;
                case '!':
                case '?':
                case '.':
                    yield return new WaitForSeconds(waitTime * 2);
                    break;
                default:
                    source[randSound].Play();
                    break;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
