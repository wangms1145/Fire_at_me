using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class playerRecoil : NetworkBehaviour
{
    public PlayerScript varibles;
    public Rigidbody2D myRigidbody;
    public playerLogic player_logic;
    [Tooltip("武器代码")]
    public Weapon_Script wp;
    public float up_comp_factor;
    public float up_comp_factor_recoil;
    private float recoil;
    //public bool heavy = false;
    void Start(){
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
        player_logic = GetComponent<playerLogic>();
        //heavy_time = heavy_time_max;
    }
    public void player_recoil(){
        if(varibles.shoot){
            recoil = varibles.recoil;
            if(player_logic.heavy)recoil /= 10;
            myRigidbody.velocity += Vector2.right * (Mathf.Cos(varibles.ang) * recoil);
            float up_recoil = (Mathf.Sin(varibles.ang) * recoil);
            myRigidbody.velocity += Vector2.up * up_recoil;
            if(myRigidbody.velocity.y < 0){
                myRigidbody.velocity += Vector2.up * Mathf.Abs(myRigidbody.velocity.y) * up_comp_factor;
                if(up_recoil > 0){
                    myRigidbody.velocity += Vector2.up * up_recoil * up_comp_factor_recoil;
                }
            }
            varibles.time_last_shoot = Time.time;
            wp.recoil_ani = (float)Math.Clamp(varibles.recoil / 6.0,0,0.7);
            varibles.shoot = false;
        }
    }
}
