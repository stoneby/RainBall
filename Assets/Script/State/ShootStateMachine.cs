using System;
using UnityEngine;

public class ShootStateMachine : AbstractContainerState
{
    protected override void OnStateEnd(object sender, EventArgs args)
    {
        Debug.Log("ShootStateMachine ends at current state: " + CurrentState);

        // This is a looping statemachine, no ending.
        if(CurrentState == EndState)
        {
            OnExit();
        }

        switch(CurrentState)
        {
            case StateType.Initializing:
                CurrentState = StateType.WaitForUserFire;
                break;
            case StateType.WaitForUserFire:
                CurrentState = StateType.Shooting;
                break;
            case StateType.Shooting:
                CurrentState = StateType.Booming;
                break;
            case StateType.Booming:
                CurrentState = StateType.EvaluateErase;
                break;
            case StateType.EvaluateErase:
                CurrentState = StatesDict[CurrentState].Pass ? StateType.Erasing : StateType.EvaluateEnding;
                break;
            case StateType.Erasing:
                CurrentState = StateType.EvaluateErase;
                break;
            case StateType.EvaluateEnding:
                CurrentState = StatesDict[CurrentState].Pass ? StateType.FinalEnding : StateType.WaitForUserFire;
                break;
            case StateType.FinalEnding:
                CurrentState = StateType.Initializing;
                break;
        }
        Go();
    }

    protected override void Awake()
    {
        Reset();
    }

    private void OnExit()
    {
        Reset();

        OnEnd();
    }

    public void Reset()
    {
        InitState = StateType.Initializing;
        CurrentState = InitState;
        EndState = StateType.FinalEnding;

        Utils.GameData.Next();
        Utils.GameData.Display();
    }
}
