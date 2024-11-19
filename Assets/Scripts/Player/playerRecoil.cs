using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRecoil : MonoBehaviour
{
    public PlayerScript varibles;
    public Rigidbody2D myRigidbody;
    public playerLogic player_logic;
    [Tooltip("武器代码")]
    public Weapon_Script wp;
    void Start(){
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
        player_logic = GetComponent<playerLogic>();
    }
    public void player_recoil(){
        if(varibles.shoot){
            myRigidbody.velocity += Vector2.right * (Mathf.Cos(varibles.ang) * varibles.recoil);
            myRigidbody.velocity += Vector2.up * (Mathf.Sin(varibles.ang) * varibles.recoil);
            varibles.time_last_shoot = Time.time;
            wp.recoil_ani = (float)Math.Clamp(varibles.recoil / 6.0,0,0.7);
            varibles.shoot = false;
        }
    }
}
