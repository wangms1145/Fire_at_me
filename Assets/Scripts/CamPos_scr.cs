using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos_scr : MonoBehaviour
{
    public PlayerScript ply;
    public float dist;
    public float spd;
    private Vector2 pos;
    private bool flag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ply.isAlive){
            pos.x = ply.disX*dist;
            pos.y = ply.disY*dist;
            transform.localPosition = pos;
            flag = true;
        }
        else{
            if(flag){
                flag = false;
                pos = transform.position;
            }
            if(Input.GetKey(KeyCode.A))pos.x -= spd*Time.deltaTime;
            if(Input.GetKey(KeyCode.D))pos.x += spd*Time.deltaTime;
            if(Input.GetKey(KeyCode.W))pos.y += spd*Time.deltaTime;
            if(Input.GetKey(KeyCode.S))pos.y -= spd*Time.deltaTime;
            transform.position = pos;
        }
    }
}
