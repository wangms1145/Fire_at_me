using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeaponScript : MonoBehaviour
{
    public DroppedWeapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        if(weapon.spr != null)GetComponent<SpriteRenderer>().sprite = weapon.spr;
    }

    // Update is called once per frame
    void Update()
    {
        if(weapon.spr != null)GetComponent<SpriteRenderer>().sprite = weapon.spr;
        if(transform.position.y < -50) GetComponent<Rigidbody2D>().simulated = false;
    }
}
