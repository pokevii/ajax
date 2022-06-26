using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnFlag : MonoBehaviour
{
    public Transform respawnPoint;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Collider2D>().CompareTag("Player")){
            gameManager.SetRespawnPoint(respawnPoint);
        }
    }
}
