using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_Script : MonoBehaviour
{
    public float pos;
    public float ang;
    
    // Start is called before the first frame update
    void Start()
    {
        ang = transform.GetChild(0).GetChild(0).rotation.eulerAngles.z + 54;
    }
    void OnEnable(){
        
        //GetComponent<Animator>().SetTrigger("UnSelect");
    }
    // Update is called once per frame
    void Update()
    {        
        transform.localPosition = Vector2.right * Mathf.Cos(ang*Mathf.Deg2Rad) * pos + Vector2.up * Mathf.Sin(ang*Mathf.Deg2Rad) * pos;
    }
}
