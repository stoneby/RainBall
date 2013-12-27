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
                CurrentState = StateType.EvaluateErase;
                break;
            case StateType.EvaluateErase:
                CurrentState = StatesDict[CurrentState].Pass
                                   ? StateType.Erasing
                                   : (Utils.GameDataManager.ShootBallIndex >= Utils.GameDataManager.CurrentShootChain.Count)
                                         ? StateType.Celebration
                                         : StateType.InitControll;
                Debug.LogWarning("--------------- shoot ball index: " + Utils.GameDataManager.ShootBallIndex);
                break;
            case StateType.Erasing:
                CurrentState = StateType.EvaluateErase;
                break;
			case StateType.Celebration:
				CurrentState = StateType.EvaluateEnding;
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

        Utils.GameDataManager.ShootBallIndex = 0;
    }
}
