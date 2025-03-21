using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    GameBaseState currentState;
    public MenuState menuState = new MenuState();
    public StartState startState = new StartState();
    public LobbyState lobbyState= new LobbyState();
    public CombatState combatState = new CombatState();
    public ShopState shopState = new ShopState();

    void Awake()
    {
        if(_instance != null && !_instance.Equals(this)){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }
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
        currentState.ExitState(this);
        string name = currentState.getName(this);
        currentState = state;
        currentState.EnterState(this);
        if(currentState.getName(this).Equals(name) == false){
            SceneManager.LoadScene(currentState.getName(this));
        }
    }
}
