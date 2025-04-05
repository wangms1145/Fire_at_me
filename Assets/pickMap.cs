using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickMap : MonoBehaviour
{
    public GameStateManager manager;
    public GameBaseState state;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void switchScene(){
        if(state != null){
            manager.SetState(state);
        }
    }
    public void game(){
        state = manager.combatState;
    }
    public void astroid(){
        state = manager.combatState;
    }
}
