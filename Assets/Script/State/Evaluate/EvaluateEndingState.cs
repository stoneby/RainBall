﻿using UnityEngine;

public class EvaluateEndingState : AbstractState
{
    public override void Go()
    {
        Debug.Log(GetType().Name + ": Go.");

        var ballList = Utils.BallManager.BallUpdaterList;
        if (ballList.Count == 0)
        {
            Debug.Log("We meet super big celebration hit, all balls are gone.");
        }

        OnEnd();
    }
}