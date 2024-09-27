using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class debug : MonoBehaviour
{
    public PlayerScript ply;
    private UnityEngine.Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = ply.sX+ply.disX;
        pos.y = ply.sY+ply.disY;
        pos.z = 0;
        transform.position = pos;
    }
}
