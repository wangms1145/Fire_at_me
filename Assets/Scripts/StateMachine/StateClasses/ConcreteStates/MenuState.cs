using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : GameBaseState
{
    public override void EnterState(GameStateManager game){
        Debug.Log("EnterMenu");
    }
    public override void ExitState(GameStateManager game){

    }
    public override void UpdateState(GameStateManager game){

    }
    public override string getName(GameStateManager game){
        return "Menu";
    }
    public override string getStateName(GameStateManager game){
        return "MenuState";
    }
}
