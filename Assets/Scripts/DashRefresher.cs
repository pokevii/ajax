using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRefresher : MonoBehaviour
{
    public float respawnTime = 4;
    private float respawnTimer;
    private bool respawning = false;
    private void Update()
    {
        if (respawning)
        {
            if(respawnTimer <= 0)
            {
                Enable();
            }

            respawnTimer -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            collision.GetComponent<Collider2D>().GetComponent<PlayerController>().RefreshDash();
            Disable();
        }
    }

    private void Disable()
    {
        respawning = true;
        respawnTimer = respawnTime;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Enable()
    {
        respawning = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
