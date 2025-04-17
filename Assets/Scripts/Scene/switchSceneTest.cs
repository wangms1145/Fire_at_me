using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class switchSceneTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameStateManager gameStateManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void switchScene(){
        gameStateManager.SetState(gameStateManager.lobbyState);
    }
}
