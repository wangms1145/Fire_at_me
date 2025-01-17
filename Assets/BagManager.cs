using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Weapon_Script weapon_Script;
    [SerializeField] private WeaponWheel weaponWheel;
    [SerializeField] private List<int> weaponInBag = new List<int>();
    [SerializeField] private int const_MaxWeponNum = 5;


    void Start()
    {
        weapon_Script = GameObject.FindObjectOfType<Weapon_Script>();
        weaponWheel = GameObject.FindObjectOfType<WeaponWheel>();
    }


    private void OnCollisionEnter2D(Collision2D other) {

        if(other.transform.tag != "DropedWeapon")
            return;

        // if ( weaponInBag.Count < 5)
        //     weaponInBag.Add(other.gameObject.GetComponent<>.GetWeapponID();)//complete this
    }

    //if the player stays on the weapon and click f, change the weapon
    private void OnCollisionStay(Collision other) {
        if(other.transform.tag != "DropedWepon")
            return;

        if(Input.GetKeyDown("z"))
        {
            //weaponInBag[weaponWheel.currentWeaponIndex] = other.gameObject.GetComponent<>.GetWeapponID();//complete this
            weapon_Script.Change(weaponWheel.currentWeaponIndex);
        }
        
    }



    //call this in weapon script
    public void DropTheWeaponAfterBulletUsedUp()
    {
        weaponInBag.Remove(weaponWheel.currentWeaponIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
