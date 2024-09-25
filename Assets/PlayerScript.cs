using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float spd;
    public float acc;
    public float aacc;
    public float jumpStrength;
    private float spdx,spdy;
    private float tspd;
    // Start is called before the first frame update
    void Start()
    {
        //myRigidbody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        spdx = myRigidbody.velocity.x;
        spdy = myRigidbody.velocity.y;
        if(Input.GetKeyDown(KeyCode.Space))myRigidbody.velocity += Vector2.up * jumpStrength;
        tspd = 0;
        if(Input.GetKey(KeyCode.A))tspd += spd;
        if(Input.GetKey(KeyCode.D))tspd -= spd;
        myRigidbody.velocity += Vector2.left * (tspd+spdx)*acc;

        myRigidbody.MoveRotation(myRigidbody.rotation+(0-myRigidbody.rotation)*aacc);
        

        
    }
}
