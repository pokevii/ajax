using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameManager gameManager;
    public Transform respawnPoint;
    public bool snapCameraToTransform = true;
    [TextArea(2, 5)]
    public string dialogueTrigger;
    bool hasTriggered = false;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            gameManager.SetRoomInfo(this);

            if (!hasTriggered)
            {
                gameManager.dialogueManager.SetActive(true);
                gameManager.UpdateDialogue(dialogueTrigger);
                hasTriggered = true;
            }
        }
    }
}
