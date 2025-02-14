
using UnityEngine;
using Unity.Services.Core;

using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;

public class TestLobby : MonoBehaviour
{
    //creating lobbies
     string createLobbyName = "This_creat_Lobby";
     int maxPlayer = 1;
     bool isCreatedLobbyPrivate = true;

    private float heartBeatTimer;

    Lobby hostLobby;

    private void Update()
    {
        HandleLobbyHeartBeat();
    }


    private void HandleLobbyHeartBeat()
    {
        if (hostLobby != null)
        {
            heartBeatTimer -= Time.deltaTime;
            if(heartBeatTimer <= 0)
            {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }


    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>{
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);

        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    public async void CreateLobby()
    {
        try{

            CreateLobbyOptions createLobbyOptions= new CreateLobbyOptions{IsPrivate = isCreatedLobbyPrivate};
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(createLobbyName, maxPlayer,createLobbyOptions);

            hostLobby = lobby; 

            Debug.Log("createdLobby!  Lobby_Name:"+ lobby.Name+"  Max_Player:"+lobby.MaxPlayers + "  ID:"+lobby.Id + "  Code:" + lobby.LobbyCode);



        }
        catch(LobbyServiceException e)
        {
            Debug.LogError("Loby Creation error:" + e);
        }

    }

    public async void ListLobbies(){
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions(){
                Count = 25,
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>(){
                    new QueryOrder(false,QueryOrder.FieldOptions.Created)
                }
                
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies Found:"+ queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
                Debug.Log("Lobby name: "+lobby.Name+"   Max_Players:"+ lobby.MaxPlayers);

            }
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError("ListLobbyError" +e);
        }
    }

    public async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
        }

        catch(LobbyServiceException e)
        {
            Debug.LogError("ListLobbyError" +e);
        }
    }
     

}
