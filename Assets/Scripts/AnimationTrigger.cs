using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;
    public string triggerToFire;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
            {
                Debug.Log("YEP HERE WE GO");
                animator.SetTrigger(triggerToFire);
                triggered = true;
            }
        }
    }
}
