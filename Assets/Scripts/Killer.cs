using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerController>().Kill();
            Debug.Log("killin the player");
        }
    }
}
