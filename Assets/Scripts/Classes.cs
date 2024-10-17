using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Classes : MonoBehaviour
{

}

[System.Serializable]
public class WeaponClass
{
    public Sprite spr;
    public float recoil;
    public float bulletSpd;
    public int type;
    public Vector2 firePos;
    public GameObject fireEff;
    public float damage;
    public int damageType;
    public float firing_time;
    public float reloading_time;
    public int bullet_count;
    public bool automatic;
    public float arm_time;
    public int mag_c;
    public int reload_type;
    public AudioClip fire_audio;
    public float spd_offset;
    public float ang_offset;
    public float ang_rec;
    public float rec_acc;
}
//What ever classes goes after this thing

