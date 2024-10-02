using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos_scr : MonoBehaviour
{
    public PlayerScript ply;
    public float dist;
    private Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = ply.disX*dist;
        pos.y = ply.disY*dist;
        transform.localPosition = pos;
    }
}
