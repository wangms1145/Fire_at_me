using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyState : GameBaseState
{
    public override void EnterState(GameStateManager game){
        Debug.Log("EnterLobby");
    }
    public override void ExitState(GameStateManager game){

    }
    public override void UpdateState(GameStateManager game){

    }
    public override string getName(GameStateManager game){
        return "Lobby";
    }
    public override string getStateName(GameStateManager game){
        return "LobbyState";
    }
}
