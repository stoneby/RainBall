using System;
using UnityEngine;

public class StateMachineGambleState : AbstractContainerState
{
    public override void Go()
    {
        Debug.Log("GameStateMachine goes at current state: " + CurrentState);
        StatesDict[CurrentState].Go();
    }

    protected override void OnStateEnd(object sender, EventArgs args)
    {
        Debug.Log("GameStateMachine ends at current state: " + CurrentState);

        switch (CurrentState)
        {
            case StateType.GameIdle:
                CurrentState = StateType.GamePlay;

                Utils.GameData.Next();
                Utils.GameData.Display();
                break;
            case StateType.GamePlay:
                CurrentState = StateType.GameEvaluate;
                break;
            case StateType.GameEvaluate:
                CurrentState = StateType.GameIdle;
                break;
        }
        Go();
    }

    protected override void Awake()
    {
        base.Awake();

        Reset();
    }

    public void Reset()
    {
        InitState = StateType.GameIdle;
        CurrentState = InitState;
    }
}
