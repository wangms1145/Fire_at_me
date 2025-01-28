using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using TMPro;
using static UnityEngine.ParticleSystem;
using Unity.Netcode;
public class PlayerScript : NetworkBehaviour
{
    [Tooltip("刚体（物理引擎）")]
    public Rigidbody2D myRigidbody;
    [Tooltip("后坐力（没事别动，这玩意会自动修改）")]
    public float recoil;
    //for debug
    [Tooltip("鼠标坐标系数（debug）")]
    public float mouse_mult;
    [Tooltip("是否活者")]
    public bool isAlive = true;
    [Tooltip("虚空y坐标")]
    public float diedYpos;
    [Tooltip("射击（别动）")]
    public bool shoot;
    //public int mag,c_mag;
    [HideInInspector]
    public float ang;
    [HideInInspector]
    public float time_last_shoot = -999;// Initialized to make sure you could shoot when ever you start the game
    [HideInInspector]
    public float mouY,mouX;
    [HideInInspector]
    public float disY,disX;
    [HideInInspector]
    public float sX,sY;
    [HideInInspector]
    public float spdx,spdy;
    [HideInInspector]
    public float angSpd;

    [HideInInspector] public int plycurrenthealth ;
    public const int kMaxHealth = 1000;

    private playerMove move;
    private playerSound sound;
    private playerRecoil player_recoil;
    private playerLogic player_logic;
    private playerCamera player_camera;
    private playerRotation rotate;


    
    
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<playerMove>();
        sound = GetComponent<playerSound>();
        player_recoil = GetComponent<playerRecoil>();
        player_logic = GetComponent<playerLogic>();
        player_camera = GetComponent<playerCamera>();
        rotate = GetComponent<playerRotation>();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.Log("player created");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) {

            
            //jump
            move.move();
            //Camera
            player_camera.cam();
            //recoil
            player_recoil.player_recoil();
            //rotation lock
            rotate.rotate();
            //logic
            player_logic.logic();
            //sound
            sound.sound();


        }
        else{
            //died
            player_logic.onDeath();
        }
    }
}
