using System;
using UnityEngine;

public class GamePlayState : AbstractContainerState
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
            case StateType.Cleaningup:
                CurrentState = StateType.Initializing;
                break;
            case StateType.Initializing:
                CurrentState = StateType.Shooting;
                break;
            case StateType.Shooting:
                CurrentState = StateType.Booming;
                break;
            case StateType.Booming:
                CurrentState = (Utils.GameData.ShootBallIndex == Utils.GameData.ShootBallList.Count - 1)
                                   ? StateType.PlayEnding
                                   : StateType.Shooting;
                Debug.Log("Current shooting ball index: " + Utils.GameData.ShootBallIndex + ", shoot balls total num: " +
                          Utils.GameData.ShootBallList.Count);
                ++Utils.GameData.ShootBallIndex;
                break;
            default:
                throw new Exception("Invalid state: " + CurrentState);
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
        InitState = StateType.Cleaningup;
        CurrentState = InitState;
        EndState = StateType.PlayEnding;

        Utils.GameData.ShootBallIndex = 0;
    }
}
