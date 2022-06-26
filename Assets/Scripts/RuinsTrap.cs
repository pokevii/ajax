using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinsTrap : MonoBehaviour
{
    public Transform[] dartSpawnLocation;
    public GameObject dart;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 0; i < dartSpawnLocation.Length; i++)
        {
            Instantiate(dart, dartSpawnLocation[i], false);
        }
    }
}
