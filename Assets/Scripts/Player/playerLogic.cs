using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Unity.Netcode;
public class playerLogic : NetworkBehaviour
{
    public PlayerScript varibles;
    public Rigidbody2D myRigidbody;
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
    [Tooltip("特效最大速度")]
    public float max_eff_spd;
    [Tooltip("特效最小速度")]
    public float min_eff_spd;
    [HideInInspector]
    public bool groundFlag;
    [HideInInspector]
    public float ys = 0;
    private int i;

    [SerializeField] private GameObject thisWeaponWheel;

    [Rpc(SendTo.ClientsAndHost)]
    public void ChangeWeaponClientRpc(int ind){
        //if(GetComponentInParent<PlayerScript>().IsOwner){return;}
        GetComponentInChildren<Weapon_Script>().ChangeWeapon(ind);

    }
    void Start(){
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.sharedMaterial = inGame_material;
        thisWeaponWheel = GameObject.FindGameObjectWithTag("WeaponWheel");
    }
    public bool isGrounded(){
        if(Physics2D.BoxCast(transform.position,boxSize,0,Vector2.down,castDistance,groundLayer)){
            return true;
        }
        else{
            return false;
        }
    }
    public void logic(){

        
        myRigidbody.sharedMaterial = inGame_material;
        Color col = Color.white;
        col.a = Mathf.InverseLerp(min_eff_spd,max_eff_spd,myRigidbody.velocity.magnitude);
        transform.GetChild(0).rotation = quaternion.RotateZ(varibles.angSpd);
        TrailModule a = transform.GetChild(0).GetComponent<ParticleSystem>().trails;
        a.colorOverTrail = col;
        transform.GetChild(1).rotation = quaternion.RotateZ(varibles.angSpd);
        a = transform.GetChild(1).GetComponent<ParticleSystem>().trails;
        a.colorOverTrail = col;

        if(transform.position.y < varibles.diedYpos || Input.GetKeyDown(KeyCode.G)){
            varibles.isAlive = false;
            float ang = UnityEngine.Random.Range(-180, 180);
            myRigidbody.velocity += Vector2.up * (float)Math.Sin(ang)*5 + Vector2.right * (float)Math.Cos(ang)*5;
            myRigidbody.angularVelocity = (float)(UnityEngine.Random.Range(-15, 15)/3.0);
            myRigidbody.sharedMaterial = died_material;
            GetComponent<playerSound>().onStartDeath();//死亡音效
        }

        if(i >= 5){ys = myRigidbody.velocity.y;i = 0;}
        i++;
        if(isGrounded() && !groundFlag){
            GetComponent<playerSound>().fallsound();//摔落音效
        }
        groundFlag = isGrounded();

            if(Input.GetMouseButtonDown(1))
            {
                thisWeaponWheel.SetActive(true);
            }

    }
    public void onDeath(){
        if(transform.position.y < varibles.diedYpos-30){
            myRigidbody.simulated = false;
        }
        if(Input.GetKey(KeyCode.R)){
            myRigidbody.simulated = true;
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.position = Vector2.zero;
            myRigidbody.rotation = 0;
            varibles.isAlive = true;
        }
    }
    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position - Vector3.up * castDistance,boxSize);
    }

    

}
