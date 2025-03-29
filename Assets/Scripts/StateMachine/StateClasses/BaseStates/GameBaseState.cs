
using UnityEngine;

public abstract class GameBaseState
{
    public abstract void EnterState(GameStateManager game);
    public abstract void ExitState(GameStateManager game);
    public abstract void UpdateState(GameStateManager game);
    public abstract string getName(GameStateManager game);
    public abstract string getStateName(GameStateManager game);
}
