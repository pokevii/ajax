using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Negative speed = moves to the left\nPositive speed = moves to the right\nNOT AFFECTED BY GRAVITY")]
    public float speed;
    public float lifetime = 5;
    private float lifetimeTimer;

    private void Start()
    {
        lifetimeTimer = lifetime;
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        if(lifetimeTimer <= 0)
        {
            Destroy(this.gameObject);
        }
        lifetimeTimer -= Time.deltaTime;    
    }
}
