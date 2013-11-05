using System;
using UnityEngine;

public class GameEvaluateState : AbstractContainerState
{
    protected override void OnStateEnd(object sender, EventArgs args)
    {
        Debug.Log(GetType().Name + ": ends at current sub state: " + CurrentState);

        if (CurrentState == EndState)
        {
            OnExit();
            return;
        }

        switch (CurrentState)
        {
            case StateType.EvaluateErase:
                CurrentState = StatesDict[CurrentState].Pass ? StateType.Erasing : StateType.EvaluateEnding;
                break;
            case StateType.Erasing:
                CurrentState = StateType.EvaluateErase;
                break;
        }
        Go();
    }

    protected override void Awake()
    {
        base.Awake();

        Reset();
    }

    private void OnExit()
    {
        OnEnd();

        Reset();
    }

    public void Reset()
    {
        InitState = StateType.EvaluateErase;
        CurrentState = InitState;
        EndState = StateType.EvaluateEnding;
    }
}
