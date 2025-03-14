
using UnityEngine;

public abstract class GameBaseState
{
    public abstract void EnterState(GameStateManager game);
    public abstract void ExitState(GameStateManager game);
    public abstract void UpdateState(GameStateManager game);
}
