using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float spd;
    public float acc;
    public float aacc;
    public float jumpStrength;
    public float recoil;
    public Cam_script cam_script;
    private float ang;
    private float spdx,spdy;
    private float tspd;
    private float mouY,mouX;
    // Start is called before the first frame update
    void Start()
    {
        //myRigidbody.freezeRotation = true;
        //transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        spdx = myRigidbody.velocity.x;
        spdy = myRigidbody.velocity.y;
        if(Input.GetKeyDown(KeyCode.Space))myRigidbody.velocity += Vector2.up * jumpStrength;
        mouX = cam_script.mousePosition.x;
        mouY = cam_script.mousePosition.y;
        ang = Mathf.Atan((mouY - transform.position.y)/(mouX - transform.position.x));
        //Debug.LogWarning((mouY - transform.position.y) + " " + (mouX - transform.position.x));
        //ang/=Mathf.PI;
        //ang*=180;
        if(mouX - transform.position.x > 0){
            ang+=Mathf.PI;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            myRigidbody.velocity += Vector2.right * (Mathf.Cos(ang) * recoil);
            myRigidbody.velocity += Vector2.up * (Mathf.Sin(ang) * recoil);
        }
        tspd = 0;
        if(Input.GetKey(KeyCode.A))tspd += spd;
        if(Input.GetKey(KeyCode.D))tspd -= spd;
        myRigidbody.velocity += Vector2.left * (tspd+spdx)*acc;

        myRigidbody.MoveRotation(myRigidbody.rotation+(0-myRigidbody.rotation)*aacc);
        

        
    }
}
