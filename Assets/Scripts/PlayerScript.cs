using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using Unity.VersionControl.Git.ICSharpCode.SharpZipLib;
using System.Text.RegularExpressions;

public class PlayerScript : MonoBehaviour
{
    [Tooltip("刚体（物理引擎）")]
    public Rigidbody2D myRigidbody;

    //player max speed
    [Tooltip("最大速度")]
    public float spd;

    //player move acceleration
    [Tooltip("加速度")]
    public float acc;

    //rotation lock acceleration
    [Tooltip("角加速度")]
    public float aacc;

    [Tooltip("跳跃力度")]
    public float jumpStrength;
    [Tooltip("是否连跳（没做）")]
    public bool DoBunnyHop;

    [Tooltip("后坐力（没事别动，这玩意会自动修改）")]
    public float recoil;
    
    //ray casting box size
    [Tooltip("地面检测碰撞箱大小")]
    public Vector2 boxSize;
    //ray casting distance
    [Tooltip("碰撞箱距离")]
    public float castDistance;
    //ray casting layermark
    [Tooltip("地面图层")]
    public LayerMask groundLayer;
    [Tooltip("游玩时物理")]
    public PhysicsMaterial2D inGame_material;
    [Tooltip("死人物理")]
    public PhysicsMaterial2D died_material;

    //for debug
    [Tooltip("鼠标坐标系数（debug）")]
    public float mouse_mult;
    [Tooltip("是否活者")]
    public bool isAlive = true;
    [Tooltip("虚空y坐标")]
    public float diedYpos;
    [Tooltip("射击（别动）")]
    public bool shoot;
    [Tooltip("玩家音频源")]
    public AudioSource audSource;
    [Tooltip("摔落音频")]
    public AudioClip fall;
    [Tooltip("死亡音频")]
    public AudioClip died;
    [Tooltip("掉虚空音效")]
    public AudioClip zhuiji;
    [Tooltip("失真效果器")]
    public AudioDistortionFilter filter;
    //public int mag,c_mag;

   
    [SerializeField]
    [Tooltip("相机代码")]
    public Cam_script cam_script;
    [Tooltip("武器代码")]
    public Weapon_Script wp;
    [HideInInspector]
    public float ang;
    private float spdx,spdy;
    private float tspd;
    [HideInInspector]
    public float time_last_shoot = -999;// Initialized to make sure you could shoot when ever you start the game
    [HideInInspector]
    public float mouY,mouX;
    [HideInInspector]
    public float disY,disX;
    [HideInInspector]
    public float sX,sY;
    private bool groundFlag;
    private float ys = 0;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody.sharedMaterial = inGame_material;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) {
            myRigidbody.sharedMaterial = inGame_material;
            spdx = myRigidbody.velocity.x;
            spdy = myRigidbody.velocity.y;

            //jump
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded()){
                myRigidbody.velocity += Vector2.up * jumpStrength;
            }

            //debug    Actually don't delete this bcs cameraPos takes that parameter.
            mouX = cam_script.mousePosition.x;
            mouY = cam_script.mousePosition.y;
            sX = transform.position.x;
            sY = transform.position.y;
            disY = (mouY - transform.position.y) * mouse_mult;
            disX = (mouX - transform.position.x) * mouse_mult;



            //recoil
            if(shoot){
                myRigidbody.velocity += Vector2.right * (Mathf.Cos(ang) * recoil);
                myRigidbody.velocity += Vector2.up * (Mathf.Sin(ang) * recoil);
                time_last_shoot = Time.time;
                wp.recoil_ani = (float)Math.Clamp(recoil / 6.0,0,0.7);
                shoot = false;
            }

            //move
            tspd = 0;
            if(Input.GetKey(KeyCode.A))tspd += spd;
            if(Input.GetKey(KeyCode.D))tspd -= spd;
            float acc_add = 0;
            if(Input.GetKey(KeyCode.LeftShift) && isGrounded()){acc_add = (float)(0.7 - acc); tspd = 0;}
            myRigidbody.velocity += Vector2.left * (tspd+spdx)*Math.Clamp((acc+acc_add)*Time.deltaTime*100,-1,1);

            //rotation lock
            myRigidbody.angularVelocity = (0-myRigidbody.rotation)*Math.Clamp(aacc*Time.deltaTime*100,-0.7f,0.7f)/Time.deltaTime;
            if(Input.GetKey(KeyCode.T)){
                myRigidbody.AddForceAtPosition(Vector2.left * 100,Vector2.up);
                myRigidbody.AddForceAtPosition(Vector2.right * 100,Vector2.down);
            }

            if(transform.position.y < diedYpos || Input.GetKeyDown(KeyCode.G)){
                isAlive = false;
                float ang = UnityEngine.Random.Range(-180, 180);
                myRigidbody.velocity += Vector2.up * (float)Math.Sin(ang)*5 + Vector2.right * (float)Math.Cos(ang)*5;
                myRigidbody.angularVelocity = (float)(UnityEngine.Random.Range(-15, 15)/3.0);
                myRigidbody.sharedMaterial = died_material;
                if(transform.position.y < diedYpos){
                    filter.distortionLevel = 0;
                    audSource.PlayOneShot(zhuiji);
                }
                else{
                    filter.distortionLevel = 0;
                    audSource.PlayOneShot(died);
                }
            }
            if(isGrounded() && !groundFlag){
                filter.distortionLevel = Mathf.InverseLerp(-10,-30,ys);
                audSource.PlayOneShot(fall);
            }
            groundFlag = isGrounded();
            if(i >= 5){ys = myRigidbody.velocity.y;i = 0;}
            i++;
        }
        else{
            //died
            mouX = cam_script.mousePosition.x;
            mouY = cam_script.mousePosition.y;
            sX = transform.position.x;
            sY = transform.position.y;
            disY = (mouY - transform.position.y) * mouse_mult;
            disX = (mouX - transform.position.x) * mouse_mult;
            if(transform.position.y < diedYpos-30){
                myRigidbody.simulated = false;
            }
            if(Input.GetKey(KeyCode.R)){
                myRigidbody.simulated = true;
                myRigidbody.velocity = Vector2.zero;
                myRigidbody.position = Vector2.zero;
                myRigidbody.rotation = 0;
                isAlive = true;
            }
        }
    }

    //ground detect using box casting
    public bool isGrounded(){
        if(Physics2D.BoxCast(transform.position,boxSize,0,Vector2.down,castDistance,groundLayer)){
            return true;
        }
        else{
            return false;
        }
    }
    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position - Vector3.up * castDistance,boxSize);
    }
}
