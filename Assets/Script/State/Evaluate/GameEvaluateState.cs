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
            case StateType.InitControll:
                CurrentState = (Utils.GameData.ShootBallIndex == Utils.GameData.ShootBallList.Count)
                                   ? StateType.EvaluateEnding
                                   : StateType.EvaluateErase;
                break;
            case StateType.EvaluateErase:
                CurrentState = StatesDict[CurrentState].Pass ? StateType.Erasing : StateType.InitControll;

                if (CurrentState == StateType.InitControll)
                {
                    ++Utils.GameData.ShootBallIndex;
                }
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
        Reset();

        OnEnd();
    }

    public void Reset()
    {
        InitState = StateType.InitControll;
        CurrentState = InitState;
        EndState = StateType.EvaluateEnding;

        Utils.GameData.ShootBallIndex = 0;
    }
}
