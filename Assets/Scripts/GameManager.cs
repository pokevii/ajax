using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    [Header("Respawning")]
    public Transform respawnLocation;
    public float respawnTime;
    private float currentRespawnTime;
    private bool playerIsRespawning;

    [Header("Dialogue")]
    public GameObject dialogueManager;
    public TextMeshProUGUI dialogueText;
    private Vector2 playerLocation;
    private string currentDialogue;
    private string lastDialogue = "afjgoisfjg";
    private bool timerSet = false;
    private float maxDialogueTime = 3;
    private float dialogueTimer;

    private bool followPlayer;


    void Start()
    {
        dialogueManager.SetActive(true);
    }

    void Update()
    {
        if (!playerIsRespawning)
        {
            playerLocation = FindObjectOfType<PlayerController>().gameObject.transform.position;
        }

        if (playerIsRespawning)
        {
            if(currentRespawnTime < 0)
            {
                Instantiate<GameObject>(player, respawnLocation);
                currentRespawnTime = respawnTime;
                playerIsRespawning = false;
            }
            else if (currentRespawnTime > 0)
            {
                currentRespawnTime -= Time.deltaTime;
            }
        } 

        if(currentDialogue == dialogueText.text)
        {
            if (!timerSet)
            {
                dialogueTimer = maxDialogueTime;
                timerSet = true;
            }

            if(dialogueTimer < 0)
            {
                dialogueManager.SetActive(false);
            }

            dialogueTimer -= Time.deltaTime;
        }

        if (followPlayer)
        {
            Camera.main.transform.position = new Vector3(playerLocation.x, Camera.main.transform.position.y, -10);
            //i know this is bad but im just using the camera follow for one part of the game

            if (Camera.main.transform.position.x < 0) Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, -10);
            if (Camera.main.transform.position.x > 80) Camera.main.transform.position = new Vector3(80, Camera.main.transform.position.y, -10);
        }
    }

    public void StartRespawnTimer()
    {
        playerIsRespawning = true;
        currentRespawnTime = respawnTime;
    }

    public void SetRespawnPoint(Transform transform)
    {
        respawnLocation = transform;
    }

    public void SetCameraPosition(Transform transform)
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    public void UpdateDialogue(string text)
    {
        if (text != "" && lastDialogue != text)
        {
            dialogueManager.SetActive(true);
            currentDialogue = text;
            dialogueText.text = text;
            dialogueManager.GetComponent<TypewriterEffect>().StartTypewriter();
            timerSet = false;
            lastDialogue = text;
        } else
        {
            Debug.Log("setting dialogue manager to false. text: " + text);
            Debug.Log("currentDialogue: " + currentDialogue);
            dialogueManager.SetActive(false);
        }
    }

    public void SetRoomInfo(Room room)
    {
        if (room.snapCameraToTransform)
        {
            SetCameraPosition(room.transform);
            followPlayer = false;
        }
        else
        {
            followPlayer = true;
        }

        SetRespawnPoint(room.respawnPoint);
    }
}
