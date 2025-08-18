using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class arrowControlScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> players = new List<GameObject>();
    [SerializeField]
    private List<GameObject> arrows = new List<GameObject>();
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private List<GameObject> tempPlayer = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        updatePlayers();

    }

    // Update is called once per frame
    void Update()
    {
        updatePlayers();
        tempPlayer.Clear();
        foreach(GameObject player in players){
            tempPlayer.Add(player);
        }
        foreach(GameObject arrow in arrows){
            if(players.Contains(arrow.GetComponent<arrowScript>().player) == false){
                arrow.SetActive(false);
                arrows.Remove(arrow);
                Destroy(arrow);
            }
            else{
                tempPlayer.Remove(arrow.GetComponent<arrowScript>().player);
            }
        }
        if(tempPlayer.Count > 0){
            foreach(GameObject player in tempPlayer){
                GameObject arrowIns = Instantiate(arrowPrefab,Vector3.zero,transform.rotation,transform);
                arrowIns.GetComponent<arrowScript>().player = player;
                arrows.Add(arrowIns);
            }
        }
        
    }
    private void updatePlayers(){
        players.Clear();
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
            if(player != null && player.GetComponent<NetworkBehaviour>().IsOwner == false){ 
                players.Add(player.gameObject);
            }
        }
    }
}
