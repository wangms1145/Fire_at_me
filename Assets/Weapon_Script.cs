using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Weapon_Script : MonoBehaviour
{
    /************************************************************
    * Read This Before You Code On This Script!                 *
    *                                                           *
    * This is our first weapon code.                            *
    * Our weapon System should be here.                         *
    * Including the weapon choosing System.                     *
    * There will be an array of images and attibutes here       *
    *************************************************************/
    public PlayerScript ply;
    private Vector2 pos;
    private Vector3 scale;
    private float ang;
    // Start is called before the first frame update
    void Start()
    {
        Change(14,(float)0.7);
    }

    // Call This method when ever you need to change the attibutes.
    void Change(float recoil,float time){// There will be much more attributes then this
        ply.recoil = recoil;
        ply.reloading_time = time;//In seconds
    }
    // Update is called once per frame
    void Update()
    {
        /*
        pos.x = ply.sX;
        pos.y = ply.sY;
        transform.position = pos;
        */

        ang = Mathf.Atan(ply.disY/ply.disX);
        scale.x = (float)-0.3;
        scale.z = (float)1;
        if(ply.disX<0){
            ang += Mathf.PI;
            scale.y = (float)-0.3;
        }
        else{
            scale.y = (float)0.3;
        }
        transform.localScale = scale;
        transform.rotation = quaternion.RotateZ(ang);
    }
}
