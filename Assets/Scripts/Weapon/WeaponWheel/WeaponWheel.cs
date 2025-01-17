using Unity.Netcode;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class WeaponWheel : NetworkBehaviour 
{
    private RectTransform canvasRectTransform;
    private Transform playerPosition;
    [SerializeField] private GameObject mousePositionPointer;
    [SerializeField] private float angleAdjustment;
    public int currentWeaponIndex;

    void Start()
    {
       this.gameObject.SetActive(false);
        Debug.Log("false");

    }

    // Update is called once per frame
    void Update()
    {   canvasRectTransform = GetComponent<RectTransform>(); 
        playerPosition = GetComponentInParent<Transform>();
        Debug.Log("1");



        Vector2 mousePositionRelativeToCenter = (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2);


        Debug.Log("2");
        //Debug.Log(localMousePosition.ToString());
        float angle = Mathf.Atan2(mousePositionRelativeToCenter.y,mousePositionRelativeToCenter.x) * Mathf.Rad2Deg;
            mousePositionPointer.transform.rotation = Quaternion.AngleAxis(angle+angleAdjustment,Vector3.forward);
        Debug.Log("3");

        if ( !Input.GetMouseButton(1))
        {
            this.gameObject.SetActive(false);
            Debug.Log("true");
        
        }
    }
}
