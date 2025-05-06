using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    public GameObject player;
    public Camera cam;
    private Vector2 diffdir;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        diffdir = player.transform.position - cam.transform.position;
        diffdir = diffdir.normalized;
        float ang = (float)Math.Atan2(diffdir.y,diffdir.x);
        diffdir = calcPos(diffdir);
        transform.localPosition = diffdir;
        transform.rotation = quaternion.RotateZ(ang);
        
    }
    private Vector2 calcPos(Vector2 dir){
        float scl1 = Screen.width/2/Math.Abs(dir.x);
        float scl2 = Screen.height/2/Math.Abs(dir.y);
        return Math.Min(scl1, scl2) * dir;
    }
}
