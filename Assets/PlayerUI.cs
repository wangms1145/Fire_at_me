using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerScript ply;
    void Start()
    {
        ply = GetComponentInParent<PlayerScript>();
        gameObject.SetActive(ply.IsOwner);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
