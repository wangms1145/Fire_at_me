using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Weapon_Script weapon_Script;
    [SerializeField] public WeaponWheel weaponWheel;
    [SerializeField] public List<int> weaponInBag = new List<int>();
    private const int kMaxWeponNum = 5;
    [SerializeField] private GameObject droppedWeaponPrefab;
    [SerializeField] private PlayerScript ply;
    private int currId = -1;

    
    public PlayerReachManager playerReachManager;


    void Start()
    {
        weapon_Script = GameObject.FindObjectOfType<Weapon_Script>();
        weaponWheel = GameObject.FindObjectOfType<WeaponWheel>();
        playerReachManager = transform.GetComponentInParent<PlayerReachManager>();
    }

    void Update()
    {
        //weaponWheel = GameObject.FindGameObjectWithTag("WeaponWheel").GetComponent<WeaponWheel>();
        if( Input.GetKeyDown(KeyCode.E) && playerReachManager.ReachableWeapons.Length > 0)//pick the wheapon
        {
            GameObject temp = playerReachManager.ReachableWeapons[0];
            if( weaponInBag.Count < kMaxWeponNum )
            {
                weaponInBag.Add(getId(temp));
                //Destroy(playerReachManager.ReachableWeapons[0]);
            }
            else
            {
                Debug.Log(weaponWheel.currentWeaponIndex);
                GameObject throwedWeapon = Instantiate(droppedWeaponPrefab, transform.position + new Vector3(ply.disX,ply.disY,0).normalized * 1.5f, Quaternion.identity);
                throwedWeapon.GetComponent<Rigidbody2D>().velocity = new Vector2(ply.disX,ply.disY).normalized * 10;
                int id = weaponInBag[weaponWheel.currentWeaponIndex];
                throwedWeapon.GetComponent<DroppedWeaponScript>().weapon.id = id;
                throwedWeapon.GetComponent<DroppedWeaponScript>().weapon.spr = weapon_Script.weapon[id].spr;
                weaponInBag[weaponWheel.currentWeaponIndex] = getId(temp);
            }
            Destroy(temp);
        }
        if(weaponWheel.currentWeaponIndex < weaponInBag.Count && weaponInBag[weaponWheel.currentWeaponIndex] != currId){
            currId = weaponInBag[weaponWheel.currentWeaponIndex];
            weapon_Script.Change(currId);
        }
    }




    // private void OnCollisionEnter2D(Collision2D other) {

    //     if(other.transform.tag != "DroppedWeapon")
    //         return;

    //     // if ( weaponInBag.Count < 5)
    //     //     weaponInBag.Add(other.gameObject.GetComponent<>.GetWeapponID();)//complete this
    // }

    // //if the player stays on the weapon and click f, change the weapon
    // private void OnCollisionStay(Collision other) {
    //     if(other.transform.tag != "DroppedWepon")
    //         return;

    //     if(Input.GetKeyDown("z"))
    //     {
    //         //weaponInBag[weaponWheel.currentWeaponIndex] = other.gameObject.GetComponent<>.GetWeapponID();//complete this
    //         weapon_Script.Change(weaponWheel.currentWeaponIndex);
    //     }
        
    // }

    private int getId(GameObject a){
        return a.GetComponent<DroppedWeaponScript>().weapon.id;
    }
    //call this in weapon script
    public void DropTheWeaponAfterBulletUsedUp()
    {
        weaponInBag.Remove(weaponWheel.currentWeaponIndex);
    }
    // Update is called once per frame

}
