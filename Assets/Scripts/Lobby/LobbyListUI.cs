using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyListUI : MonoBehaviour
{

    [SerializeField] private Transform lobbySingleTemplate;

    void Start()
    {
        
    }

    private void Awake() 
    {
        lobbySingleTemplate.gameObject.SetActive(false);
        //createLobbyButton.onClick.AddListener(CreateLobbyButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
