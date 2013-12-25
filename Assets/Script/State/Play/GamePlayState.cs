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
                // prepare hitting ball list.
                Utils.GameDataManager.CurrentShootChain.ForEach(
                    shootPair => Utils.Shooter.HitBallList.Add(Utils.BallChainManager.BallUpdaterList[shootPair.Location].gameObject));
                CurrentState = StateType.PreShooting;
                break;
            case StateType.PreShooting:
                CurrentState = StateType.Shooting;
                break;
            case StateType.Shooting:
                CurrentState = StateType.Booming;
                break;
            case StateType.Booming:
                CurrentState = (Utils.GameDataManager.ShootBallIndex == Utils.GameDataManager.CurrentShootChain.Count - 1)
                                   ? StateType.PlayEnding
                                   : StateType.PreShooting;
                Debug.Log("Current shooting ball index: " + Utils.GameDataManager.ShootBallIndex + ", shoot balls total num: " +
                          Utils.GameDataManager.CurrentShootChain.Count);
                ++Utils.GameDataManager.ShootBallIndex;
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
        Reset();
        
        OnEnd();
    }

    public void Reset()
    {
        InitState = StateType.Cleaningup;
        CurrentState = InitState;
        EndState = StateType.PlayEnding;

        Utils.GameDataManager.ShootBallIndex = 0;
    }
}
