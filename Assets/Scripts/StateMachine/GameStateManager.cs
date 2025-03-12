using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    GameBaseState currentState;
    MenuState menuState = new MenuState();
    StartState startState = new StartState();
    LobbyState lobbyState= new LobbyState();
    CombatState combatState = new CombatState();
    ShopState shopState = new ShopState();
    

    // Start is called before the first frame update
    void Start()
    {
        SetState(menuState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }
    public void SetState(GameBaseState state){
        currentState = state;
        currentState.EnterState(this);
    }
}
