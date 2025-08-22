using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

//One instance of this class represents logic for player recoil
public class playerRecoil : NetworkBehaviour
{
    public PlayerScript varibles;
    public Rigidbody2D myRigidbody;
    public playerLogic player_logic;
    public playerMove player_move;
    [Tooltip("武器代码")]
    public Weapon_Script wp;
    public float up_comp_factor;
    public float up_comp_factor_recoil;
    private NetworkVariable<Vector2> speedNet = new NetworkVariable<Vector2>(
                                                    new Vector2(0, 0),
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Server
                                                    );
    private float recoil;
    //public bool heavy = false;
    void Start()
    {
        varibles = GetComponent<PlayerScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
        player_logic = GetComponent<playerLogic>();
        player_move = GetComponent<playerMove>();
        //heavy_time = heavy_time_max;
    }
    public void player_recoil()
    {
        if (varibles.shoot)
        {
            recoil = varibles.recoil;
            if (player_logic.heavy) recoil /= 10;
            addSpd(Vector2.right * (Mathf.Cos(varibles.ang) * recoil));
            float up_recoil = Mathf.Sin(varibles.ang) * recoil;
            addSpd(Vector2.up * up_recoil);
            if (speedNet.Value.y < 0)
            {
                addSpd(Vector2.up * Mathf.Abs(speedNet.Value.y) * up_comp_factor);
                if (up_recoil > 0)
                {
                    addSpd(Vector2.up * up_recoil * up_comp_factor_recoil);
                }
            }
            varibles.time_last_shoot = Time.time;
            wp.recoil_ani = (float)Math.Clamp(varibles.recoil / 6.0, 0, 0.7);
            varibles.shoot = false;
        }
    }
    void Update()
    {
        if (IsServer)
        {
            speedNet.Value = myRigidbody.velocity;
        }
    }
    private void addSpd(Vector2 spd)
    {
        player_move.addSpdRPC(spd);
    }

}
