using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class debug : MonoBehaviour
{
    /*******************************************************************
    * Read This first before changing this code!!!!                    *
    *                                                                  *
    * This code is for debuging mouse position only                    *
    *                                                                  *
    * PLS create a new script for the aim point and crosshair!!!!!!    *
    ********************************************************************/
    public PlayerScript ply;
    private UnityEngine.Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ply.isAlive){
            //displaying mouse pos using the player as an origin
            pos.x = ply.disX;
            pos.y = ply.disY;
            pos.z = 0;
            transform.localPosition = pos;
        }
        else{
            //died
        }
    }
}
