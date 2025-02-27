using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float pos;
    public Vector3 hori;
    public Vector3 vert;
    public Vector3 depth;
    public Vector3 center;
    public float rpm;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = center + hori * Mathf.Cos(pos) + vert * Mathf.Sin(pos) + depth * Mathf.Sin(pos);
        pos += Time.deltaTime * rpm / 60 * Mathf.PI * 2;
        if(pos > Math.PI * 2){
            pos -= (float)(Math.PI * 2);
        }
        if(transform.position.z > center.z){
            GetComponent<SpriteRenderer>().sortingOrder = -4;
        }
        else{
            GetComponent<SpriteRenderer>().sortingOrder = -2;
        }
    }
}
