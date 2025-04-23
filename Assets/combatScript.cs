using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class combatScript : MonoBehaviour
{
    public static List<GameObject> spawns;
    public static combatScript _instance;
    void Awake()
    {
        if(_instance != null && _instance != this){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if(_instance == null){
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(NetworkManager.Singleton.IsClient){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
