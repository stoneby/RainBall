using System;
using UnityEngine;

public class InitializingState : AbstractState
{
    // Use this for initialization
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        Utils.BallGenerator.Generate();
        var leaderBall = Utils.BallManager.BallUpdaterList[0];
        Utils.MoveLevel(leaderBall.gameObject, Utils.Settings.MoveSpeed);
    }

    private void OnStopMoving(object sender, EventArgs args)
    {
        OnEnd();
    }

    protected override void Start()
    {
        base.Start();

        Utils.BallManager.StopMoving += OnStopMoving;
    }
}
