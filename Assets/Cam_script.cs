using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Cam_script : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    public VC_Script vc;
    public Vector2 mousePosition;
    public int scr_x,scr_y;
    public float debug_mult;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }



    // change the mouse position relative to the world. Provided by the Unity. - Luke
    void OnGUI()
    {
        Vector3 point = new Vector3();
        Event   currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        mousePosition.x = (point.x - transform.position.x)*debug_mult + transform.position.x;
        mousePosition.y = (point.y - transform.position.y)*debug_mult + transform.position.y;
        scr_x = cam.pixelWidth;
        scr_y = cam.pixelHeight;
        
        //delete this if you don't want is to appear on screen.
        //****************************************************************************************************************************************************************
        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();
        //****************************************************************************************************************************************************************
    }
}
