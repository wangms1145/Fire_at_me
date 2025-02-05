using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet_Remain : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text text;
    [SerializeField] private Weapon_Script wp;
    private WeaponClass weapon;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        weapon = wp.GetWeapon();
        if(!weapon.infinite){
            text.text = wp.GetLeft() + "/" + wp.GetMag();
        }
    }
}
