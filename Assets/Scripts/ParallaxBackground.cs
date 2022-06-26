using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    //thx dani
    public float length, startPos;
    public float parallaxStrength;
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (mainCam.transform.position.x * parallaxStrength);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
