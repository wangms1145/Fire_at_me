using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using UnityEngine;

public class VC_Script : MonoBehaviour
{
    [SerializeField]
    public Cam_script cam_script;
    public CinemachineVirtualCamera myVCam;
    public float mult,slw_rt;
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
        mX = Input.mousePosition.x;
        mY = Input.mousePosition.y;
        sX = cam_script.scr_x/2;
        sY = cam_script.scr_y/2;
        dis = 0;
        dis += Mathf.Pow((mX-sX)/sX,2);
        dis += Mathf.Pow((mY-sY)/sY,2);
        dis = Mathf.Sqrt(dis);
        dis *= mult;
        ang = Mathf.Atan(dis/1);
        ang /= Mathf.PI;
        ang *= 180;
        if(ang < 20){
            ang = 20;
        }
        myVCam.m_Lens.FieldOfView += (ang-myVCam.m_Lens.FieldOfView)*slw_rt;
    }
}
