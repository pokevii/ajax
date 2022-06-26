using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    private AudioManager audioManager;

    public void PlaySound(string s)
    {
        audioManager.Play(s);
    }
}
