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
    public float maxDistance;
    public int type;
    public Vector2 firePos;
    public Sprite fireEff;
    public float damage;
    public int damageType;
    public float firing_time;
    public float reloading_time;
    public int bullet_count;
    public bool automatic;
    public float arm_time;
    public int mag_c;
    public int reload_type;
}
public class Bullets // I guess it is a better idea to change this stuff to a gam object but idk how to do it yet
{                    // So just don't use it
    public Sprite bullet;
    public float bulletSpd;
    public float maxDistance;
    public float trigDistance;
    public int type;
    public float damage;
    public int damageType;
}
//What ever classes goes after this thing

