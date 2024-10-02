using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentClass : MonoBehaviour
{

}
public class Classes : ParentClass
{

}

[System.Serializable]
public class WeaponClass
{
    Sprite spr;
    float recoil;
    float bulletSpd;
    float maxDistance;
    int type;
    Vector2 firePos;
    Sprite fireEff;
    float damage;
    int damageType;
}
public class Bullets // I guess it is a better idea to change this stuff to a gam object but idk how to do it yet
{                    // So just don't use it
    Sprite bullet;
    Vector2 launchPos;
    float bulletSpd;
    float maxDistance;
    float trigDistance;
    int type;
    float damage;
    int damageType;
    
}
//What ever classes goes after this thing

