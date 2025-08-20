using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_QuickJoinLobby : MonoBehaviour
{

    public Button myButton;
    void Start()
    {

        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(() => TestLobby.Instance.QuickJoinLobby());
        
    }
}
