using Unity.Netcode;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class ArmsRoulette : NetworkBehaviour 
{
    private RectTransform canvasRectTransform;
    private Transform playerPosition;
    [SerializeField] private GameObject mousePositionPointer;
    [SerializeField] private float angleAdjustment;

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

        // Vector2 localMousePosition;

        //         RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //     canvasRectTransform,
        //     Input.mousePosition,
        //     Camera.main,
        //     out localMousePosition
        // );

        //         Vector3 mousePos = Input.mousePosition;   
        // mousePos.z=Camera.main.nearClipPlane;
        // Vector3 Worldpos=Camera.main.ScreenToWorldPoint(mousePos);  
        // Vector2 localMousePosition = new Vector2(Worldpos.x,Worldpos.y);
        // Debug.Log(playerPosition.ToString() );
        // localMousePosition.x -= playerPosition.position.x;
        // localMousePosition.y -= playerPosition.position.y;


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
