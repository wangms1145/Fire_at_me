using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using UnityEngine;

public class VC_Script : MonoBehaviour
{
    [SerializeField]
    public Cam_script cam_script;
    public PlayerScript ply;
    public CinemachineVirtualCamera myVCam;
    //size of camera view
    public float mult;
    //angle acceleration of camera FOV
    public float slw_rt;
    public float spec_slw_rt;
    public float spec_scl_rt;
    public float FOV;
    //minimum FOV
    public float min_FOV;
    //private double dis;
    private float ang,dis;
    private float mX,mY,sX,sY;
    // Start is called before the first frame update
    void Start()
    {
        myVCam.m_Lens.FieldOfView = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(ply != null && ply.isAlive){
            //getting positions
            mX = Input.mousePosition.x;
            mY = Input.mousePosition.y;
            sX = cam_script.scr_x/2;
            sY = cam_script.scr_y/2;

            //calculating distance to center of screen
            dis = 0;
            dis += Mathf.Pow((mX-sX)/sX,2);
            dis += Mathf.Pow((mY-sY)/sY,2);
            dis = Mathf.Sqrt(dis);
            dis *= mult;

            //claculating FOV
            ang = Mathf.Atan(dis/1);
            ang /= Mathf.PI;
            ang *= 180;
            if(ang < min_FOV){
                ang = min_FOV;
            }

            //set FOV
            myVCam.m_Lens.FieldOfView += (ang-myVCam.m_Lens.FieldOfView)*slw_rt*Time.deltaTime;
            FOV = myVCam.m_Lens.FieldOfView;
        }
        else{
            if(Input.GetKey(KeyCode.Q))ang += spec_scl_rt*Time.deltaTime;
            if(Input.GetKey(KeyCode.E))ang -= spec_scl_rt*Time.deltaTime;
            ang = Math.Clamp(ang, min_FOV , 70);
            myVCam.m_Lens.FieldOfView += (ang-myVCam.m_Lens.FieldOfView)*spec_slw_rt*Time.deltaTime;
            FOV = myVCam.m_Lens.FieldOfView;
        }
    }
}
