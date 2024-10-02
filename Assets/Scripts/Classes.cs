using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentClass : MonoBehaviour
{

}
public class Classes : ParentClass
{
    WeaponClass[] weapon;
    private void start(){
        
    }
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
}
public class Bullets
{
    Sprite bullet;
    Vector2 launchPos;
    float bulletSpd;
    float maxDistance;
    float trigDistance;
    int type;
}

