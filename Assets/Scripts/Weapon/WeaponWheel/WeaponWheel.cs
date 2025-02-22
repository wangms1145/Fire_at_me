using Unity.Netcode;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;


public class WeaponWheel : NetworkBehaviour 
{
    private RectTransform canvasRectTransform;
    private Transform playerPosition;
    [SerializeField] private GameObject mousePositionPointer;
    [SerializeField] private GameObject[] menus = new GameObject[5];
    private bool[] menuflag = new bool[5];
    [SerializeField] private float angleAdjustment;
    [SerializeField] private Weapon_Script weapon_Script;
    [SerializeField] private Sprite Empty;
    public int currentWeaponIndex = 1,id=1;
    public float scaleFactor;
    private Vector2 mousePositionRelativeToCenter;
    private float angle;
    private const float kAngOff = 90;
    private const float kAng = 72;//both in degrees
    private bool flag = false;
    private int[] wps;




    void Start()
    {
        menus = GameObject.FindGameObjectsWithTag("UImenu");
        Debug.Log("false");
        canvasRectTransform = GetComponent<RectTransform>(); 
        playerPosition = GetComponentInParent<Transform>();
        GameObject.FindGameObjectWithTag("BAG").GetComponent<BagManager>().weaponWheel = this;
        flag = true;


        //gameObject.SetActive(false);
    }



    void OnEnable(){
        if(menus[0] == null) return;
        wps = GameObject.FindGameObjectWithTag("BAG").GetComponent<BagManager>().weaponInBag.ToArray();
        for(int i = 0 ; i < 5 ; i++){
            menus[i].GetComponent<Animator>().SetTrigger("Reset");
            menuflag[i] = false;
            if(i < wps.Length){
                menus[i].transform.GetChild(1).GetComponent<Image>().sprite = weapon_Script.weapon[wps[i]].spr;
                menus[i].transform.GetChild(1).GetComponent<Image>().SetNativeSize();
            }
            else{
                menus[i].transform.GetChild(1).GetComponent<Image>().sprite = Empty;
            }
        }
    }



    void Update()
    {   
        //Debug.Log("1");


        mousePositionRelativeToCenter = (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2);


        //Debug.Log("2");
        //Debug.Log(localMousePosition.ToString());
        angle = (float)vecToAng(mousePositionRelativeToCenter) * Mathf.Rad2Deg;
            mousePositionPointer.transform.rotation = Quaternion.AngleAxis(angle+angleAdjustment,Vector3.forward);
        //Debug.Log("3");

        if ( !Input.GetMouseButton(1))
        {
            this.gameObject.SetActive(false);
            //Debug.Log("true");
        
        }
        for(int i = 1,ac; i <= 5 ; i++){
            ac = menuActivateInd(angle);
            if(ac == i){
                if(!menuflag[i-1])menus[i-1].GetComponent<Animator>().SetTrigger("OnSelect");
                id = ac;
            }
            else{
                if(menuflag[i-1])menus[i-1].GetComponent<Animator>().SetTrigger("OnUnSelect");
            }
            menuflag[i-1] = ac==i;
        }
    }
    private void LateUpdate() {
        if(flag){
            gameObject.SetActive(false);
            flag = false;
        }
    }
    void OnDisable() {
        currentWeaponIndex = id-1 < 0 ? 0 : id-1;
        GameObject.FindGameObjectWithTag("BAG").GetComponent<BagManager>().weaponWheel = this;
    }





    private double vecToAng(Vector2 pos){
        return Mathf.Atan2(pos.y,pos.x);
    }




    private int menuActivateInd(float ang){
        ang-=kAngOff;
        ang+=kAng/2;
        if(ang >= 360){
            ang -= 360;
        }
        if(ang < 0){
            ang += 360;
        }
        
        return (int)(ang/kAng)+1;
    }
}
